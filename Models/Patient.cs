using System;
using System.Collections.Generic;
using Npgsql;

namespace Element
{
    public class Patient
    {
        public string id_patient { get; set; }
        public string name { get; set; }
        public List<PatientTooth> teeth { get; set; }

        public Patient()
        {
        }

        public Patient(string idPatient, string name, List<PatientTooth> teeth)
        {
            id_patient = idPatient;
            this.name = name;
            this.teeth = teeth;
        }

        public static List<Patient> Select(NpgsqlConnection conn)
        {
            List<Patient> results = new List<Patient>();
            List<Patient> AllResults = new List<Patient>();

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM patient";
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read()){
                        Patient patient = new Patient
                        {
                            id_patient = reader["id_patient"].ToString(),
                            name = reader["name"].ToString(),
                        };

                        results.Add(patient);
                    }
                    reader.Close();
                }
            }

            foreach (Patient result in results)
            {
                result.teeth = PatientTooth.Select(conn, result.id_patient);
                AllResults.Add(result);
            }

            return AllResults;
        }

        public static Patient SelectById(NpgsqlConnection conn, string idPatient , string priority)
        {
            Patient result = new Patient();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * FROM patient WHERE id_patient = @IdPatient";
                cmd.Parameters.AddWithValue("@IdPatient", idPatient);

                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Patient patient = new Patient
                        {
                            id_patient = reader["id_patient"].ToString(),
                            name = reader["name"].ToString(),
                        };
                        result = patient;
                    }
                }
            }

            result.teeth = PatientTooth.Select(conn, result.id_patient);
            return result.OrderByPriority(priority);
        }

        public Patient OrderByPriority(string priority)
        {
            switch (priority.ToLower())
            {
                case "health":
                    this.teeth = this.teeth.OrderBy(pt => pt.tooth.health_priority).ToList();
                    break;
                case "beauty":
                    this.teeth = this.teeth.OrderBy(pt => pt.tooth.beauty_priority).ToList();
                    break;
                default:
                    break;
            }
            return this;
        }
    }
}
