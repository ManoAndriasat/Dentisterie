


using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Element;
using System;
using Utils.helper;
using Newtonsoft.Json;
using System.Text.Json;

public class PatientController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Insert([FromBody] List<PatientTooth> patientData)
    {
        try
        {
            using (NpgsqlConnection conn = new Connection().GetConnection())
            {
                Console.WriteLine("JSON reçu :");
                Console.WriteLine(JsonConvert.SerializeObject(patientData, Formatting.Indented));
                foreach (var patientTooth in patientData)
                {
                    PatientTooth.Insert(conn, patientTooth);
                }
            }

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return BadRequest($"Erreur lors de l'insertion : {ex.Message}");
        }
    }

    public IActionResult Note(string id_patient, string id_tooth, string condition , DateTime date_visit){
        NpgsqlConnection conn = new Connection().GetConnection();

            string[] conditionsArray = condition.Split(';');
            string[] idToothArray = id_tooth.Split(';');

            if(conditionsArray.Length == idToothArray.Length){
                for(int i = 0; i < conditionsArray.Length; i++){
                    PatientTooth patientTooth = new PatientTooth();
                    patientTooth.id_patient = id_patient;
                    patientTooth.id_tooth = idToothArray[i];
                    patientTooth.condition = Int32.Parse(conditionsArray[i]);
                    patientTooth.date_visit = date_visit;
                    PatientTooth.Insert(conn, patientTooth);
                }
            }

            if(conditionsArray.Length == 1){
                for(int i = 0; i < idToothArray.Length; i++){
                    PatientTooth patientTooth = new PatientTooth();
                    patientTooth.id_patient = id_patient;
                    patientTooth.id_tooth = idToothArray[i];
                    patientTooth.condition = Int32.Parse(condition);
                    patientTooth.date_visit = date_visit;
                    PatientTooth.Insert(conn, patientTooth);
                }
            }

        return RedirectToAction("Index");
    }
}


