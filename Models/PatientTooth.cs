using System;
using System.Collections.Generic;
using Npgsql;
using Utils.helper;

namespace Element
{
    public class PatientTooth
    {
        public string id_patient { get; set; }
        public string id_tooth { get; set; }
        private int _condition;
        public DateTime date_visit { get; set; }
        public Tooth tooth { get; set; }

        public int condition
        {
            get { return _condition; }
            set
            {
                if (value < 0 || value > 11)
                {
                    throw new ArgumentException("La valeur de condition doit être positive.");
                }
                _condition = value;
            }
        }
       
        public PatientTooth()
        {
        }

        public PatientTooth(string idPatient, string idTooth, int condition, DateTime dateVisit, Tooth t)
        {
            id_patient = idPatient;
            id_tooth = idTooth;
            this.condition = condition;
            date_visit = dateVisit;
            tooth = t;
        }

        public PatientTooth(string idPatient, string idTooth, int condition, DateTime dateVisit)
        {
            id_patient = idPatient;
            id_tooth = idTooth;
            this.condition = condition;
            date_visit = dateVisit;
        }

        public static void Insert(NpgsqlConnection conn, PatientTooth patientTooth)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id_patient", patientTooth.id_patient },
                { "id_tooth", patientTooth.id_tooth },
                { "condition", patientTooth.condition },
                { "date_visit", patientTooth.date_visit }
            };

            DatabaseHelper.Insert(conn, "patient_tooth", data);
        }

        public static List<PatientTooth> Select(NpgsqlConnection conn, string idPatient)
        {
            List<PatientTooth> results = new List<PatientTooth>();
            List<PatientTooth> allResults = new List<PatientTooth>();

            try
            {
                string query = @"
                    SELECT DISTINCT id_patient, id_tooth, condition, date_visit
                    FROM (
                        SELECT id_patient, id_tooth, condition, date_visit,
                            ROW_NUMBER() OVER (PARTITION BY id_patient, id_tooth ORDER BY date_visit DESC) as row_num
                        FROM patient_tooth
                        WHERE id_patient = @idPatient
                    ) AS subquery
                    WHERE row_num = 1
                    ORDER BY id_tooth;
                ";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idPatient", idPatient);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatientTooth patientTooth = new PatientTooth
                            {
                                id_patient = reader["id_patient"].ToString(),
                                id_tooth = reader["id_tooth"].ToString(),
                                condition = Convert.ToInt32(reader["condition"]),
                                date_visit = Convert.ToDateTime(reader["date_visit"])
                            };
                            results.Add(patientTooth);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Gérer l'exception (log, affichage, etc.)
                Console.WriteLine(ex.Message);
            }

            foreach (PatientTooth patientTooth in results)
            {
                patientTooth.tooth = Tooth.SelectById(conn, patientTooth.id_tooth);
                allResults.Add(patientTooth);
            }

            return allResults;
        }

        public static PatientTooth SelectById(NpgsqlConnection conn, string idPatient,string idTooth)
        {
            List<PatientTooth> results = new List<PatientTooth>();

            try
            {
                string query = "SELECT id_patient, id_tooth, condition, MAX(date_visit) as date_visit_max FROM patient_tooth WHERE id_tooth= @idTooth and id_patient = @idPatient GROUP BY id_patient, id_tooth, condition";

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("idPatient", idPatient);
                    cmd.Parameters.AddWithValue("idTooth", idTooth);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PatientTooth patientTooth = new PatientTooth
                            {
                                id_patient = reader["id_patient"].ToString(),
                                id_tooth = reader["id_tooth"].ToString(),
                                condition = Convert.ToInt32(reader["condition"]),
                                date_visit = Convert.ToDateTime(reader["date_visit_max"])
                            };
                            results.Add(patientTooth);
                        }
                    }
                }
            }
            catch (Exception ex) { }
            results[0].tooth = Tooth.SelectById(conn, results[0].id_tooth);
            return results[0];
        }
    }
}
