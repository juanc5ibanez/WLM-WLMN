using System;
using System.Collections.Generic;
using Cassandra;
using WLM_WLMN.Common.DTOs;
using WLM_WLMN.Common.Interfaces;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WLM_WLMN.CassandraStorage
{
    public class UserUpdateStorage : IUserUpdateStorage
    {
        private ISession _session;
        public UserUpdateStorage()
        {
            _session = Common.Session;
            _session.Execute("CREATE TABLE IF NOT EXISTS userupdates( InternalID uuid ,content varchar,ExtractionDate timestamp,Longitude double, Latitude double,source varchar,term varchar,location varchar, PRIMARY KEY(InternalID));");
        }
        public void Save(UserUpdate userUpdate)
        {
            var insertPreparation =_session.Prepare("insert into wlm.userupdates (InternalID ,content ,ExtractionDate ,Longitude , Latitude ,source ,term,location ) VALUES(?,?,?,?,?,?,?,?)");
            var insertBinding = insertPreparation.Bind(userUpdate.InternalID,userUpdate.Content,userUpdate.ExtractionDate,userUpdate.Longitude,userUpdate.Latitude,userUpdate.Source,userUpdate.Term,userUpdate.Location);
            _session.Execute(insertBinding);
        }

        public void Backup(string targetLocation, int numberOfUpdates)
        {
            if(!Directory.Exists(targetLocation))
                throw new Exception("target path is not a valid directory.");
            int fileCounter = 0;
            int rowCounter = 0;
            var streamWriter = GetStreamWriter(targetLocation, ref fileCounter);
            var dbResult = _session.Execute("select InternalID ,content ,ExtractionDate ,Longitude , Latitude ,source ,term, location from wlm.userupdates;");
            JArray jArray =new JArray();
            foreach (Row row in dbResult)
            {
                var userUpdate = new UserUpdate()
                {
                    InternalID = row.GetValue<Guid>("internalid"),
                    Content = row.GetValue<string>("content"),
                    ExtractionDate = row.GetValue<DateTime>("extractiondate"),
                    Longitude = row.GetValue<double?>("longitude"),
                    Latitude = row.GetValue<double?>("latitude"),
                    Source = row.GetValue<string>("source"),
                    Term = row.GetValue<string>("term"),
                    Location = row.GetValue<string>("location")
                };
                jArray.Add(JObject.FromObject( userUpdate));
                //streamWriter.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", row.GetValue<Guid>("internalid"), row.GetValue<string>("content"), row.GetValue<DateTime>("extractiondate"), row.GetValue<double?>("longitude"), row.GetValue<double?>("latitude"), row.GetValue<string>("source"), row.GetValue<string>("term"), row.GetValue<string>("location"));
                rowCounter++;
                if (rowCounter == numberOfUpdates)
                {
                    WriteJArray(streamWriter, jArray);
                    CloseStream(streamWriter);
                    streamWriter = GetStreamWriter(targetLocation, ref fileCounter);
                    rowCounter = 0;
                }
            }
            WriteJArray(streamWriter, jArray);
            CloseStream(streamWriter);
        }

        public void Restore(DirectoryInfo directoryInfo)
        {
            _session.Execute("truncate wlm.userupdates");
            foreach (FileInfo fileInfo in directoryInfo.GetFiles().Where(x=>x.Extension.EndsWith("json")))
            {
                List<UserUpdate> userUpdates =
                    JsonConvert.DeserializeObject<List<UserUpdate>>(
                        File.ReadAllText(fileInfo.FullName));
                userUpdates.ForEach(Save);
            }
        }

        #region Private methods
        private static void WriteJArray(StreamWriter streamWriter, JArray jArray)
        {
            streamWriter.Write(jArray.ToString());
            jArray.Clear();
        }

        private static void CloseStream(StreamWriter streamWriter)
        {
            streamWriter.Flush();
            streamWriter.Close();
            streamWriter.Dispose();
        }

        

        private StreamWriter GetStreamWriter(string targetLocation, ref int fileCounter)
        {
            fileCounter++;
            string filePath = targetLocation + "\\" + fileCounter+".json";
            if (File.Exists(filePath))
                File.Delete(filePath);
            StreamWriter streamWriter = new StreamWriter(filePath);
            return streamWriter;
        }

        #endregion
    }
}
