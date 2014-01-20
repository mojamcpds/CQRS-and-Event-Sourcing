using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using CQRS.Bus;
using System.Data.SqlClient;
using CQRS.Core;
using System.IO;
using System.Configuration;

namespace CQRS.EventStore
{
    public class SqlServerEventStore:IEventStore
    {
        private readonly IEventBus bus;
		private readonly IFormatter formatter;

        //private SqlConnection _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        public SqlServerEventStore(IEventBus bus, IFormatter formatter)
		{
			this.bus = bus;
	        this.formatter = formatter;
		}
        
		public void SaveEvents(Guid aggregateId, IEnumerable<Event> events, int expectedVersion)
		{
			List<EventDescriptor> eventDescriptors = GetEventsByAggregateId(aggregateId);
			
			if(eventDescriptors.Any() && eventDescriptors.Last().Version != expectedVersion && expectedVersion != -1)
			{
				throw new ConcurrencyException();
			}

			var i = expectedVersion;
            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
				{
                    try
                    {
                        foreach (var @event in events)
                        {
                            i++;
                            @event.Version = i;
                            SaveEvent(new EventDescriptor(aggregateId, @event, i), sqlTransaction);
                            this.bus.Publish(@event);
                        }
                        sqlTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
				}
            }
		}

        private void SaveEvent(EventDescriptor eventDescriptor, SqlTransaction transaction)
        {
            const string commandText = "INSERT INTO Events VALUES(@aggregateId, @eventData, @version)";
            using (var sqlCommand = new SqlCommand(commandText, transaction.Connection, transaction))
            {
                sqlCommand.Parameters.Add(new SqlParameter("@aggregateId", eventDescriptor.AggregateId));
                sqlCommand.Parameters.Add(new SqlParameter("@eventData", Serialize(eventDescriptor.Event)));
                sqlCommand.Parameters.Add(new SqlParameter("@version", eventDescriptor.Version));

                sqlCommand.ExecuteNonQuery();
            }
        }

		private List<EventDescriptor> GetEventsByAggregateId(Guid aggregateId)
		{
			var eventDescriptors = new List<EventDescriptor>();

            string commandText = @"SELECT eventData, version FROM Events WHERE aggregateId = @aggregateId ORDER BY Version ASC;";

            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                sqlConnection.Open();

                using (var sqlTransaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        using (var sqlCommand = new SqlCommand(commandText, sqlTransaction.Connection, sqlTransaction))
                        {
                            sqlCommand.Parameters.Add(new SqlParameter("@aggregateId", aggregateId));
                            using (var sqlDataReader = sqlCommand.ExecuteReader())
                            {
                                while (sqlDataReader.Read())
                                {
                                	var eventDescriptor = new EventDescriptor(
                                		aggregateId,
                                        this.Deserialize<Event>((byte[])sqlDataReader["eventData"]),
                                        (int)sqlDataReader["version"]);
                                    eventDescriptors.Add(eventDescriptor);
                                }
                            }
                        }
                        sqlTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        sqlTransaction.Rollback();
                        throw;
                    }
                }
            }
            return eventDescriptors;
		}

		public  List<Event> GetEventsForAggregate(Guid aggregateId)
		{
			List<EventDescriptor> eventDescriptors = GetEventsByAggregateId(aggregateId);
			if (eventDescriptors.Count == 0)
			{
				throw new AggregateNotFoundException();
			}
			   
			return eventDescriptors.Select(desc => desc.Event).ToList();
		}

		private byte[] Serialize(object theObject)
		{
			using (var memoryStream = new MemoryStream())
			{
				formatter.Serialize(memoryStream, theObject);
				return memoryStream.ToArray();
			}
		}

		private TType Deserialize<TType>(byte[] bytes)
		{
			using (var memoryStream = new MemoryStream(bytes))
			{
				return (TType)formatter.Deserialize(memoryStream);
			}
		}
    }
}
