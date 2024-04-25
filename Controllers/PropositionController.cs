using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Utils.helper;
using Element;

public class PropositionController : Controller
{
    public IActionResult Index()
    {
        NpgsqlConnection connection = new Connection().GetConnection();
        List<Patient> patients = Patient.Select(connection);
        List<Element.Action> listAction = new List<Element.Action>();

        if (HttpContext.Session.GetString("id_patient") == null){
            HttpContext.Session.SetString("id_patient", patients[0].id_patient);}
        string id_patient = HttpContext.Session.GetString("id_patient");

        if (HttpContext.Session.GetString("priorite") == null){
            HttpContext.Session.SetString("priorite", "health");}
        string priorite = HttpContext.Session.GetString("priorite");

        Patient patient = Patient.SelectById(connection, id_patient,priorite);

        if (HttpContext.Session.GetString("budget") != null)
        {
            double budget = double.Parse(HttpContext.Session.GetString("budget"));
            listAction = Element.Action.ProcessPatientTeeth(patient.teeth, budget);
            ViewBag.ListAction = listAction;
        }

        if (HttpContext.Session.GetString("id_tooth") != null && HttpContext.Session.GetString("budget")=="0")
        {
            string id_tooth=HttpContext.Session.GetString("id_tooth");
            PatientTooth PT = PatientTooth.SelectById(connection,id_patient,id_tooth);
            double total = PT.tooth.repair_price*3 + PT.tooth.removal_price +PT.tooth.cleaning_price*3 + PT.tooth.replacement_price*3;
            List<Element.Action> simpleAction= Element.Action.Proposition(PT, total);
            ViewBag.ListAction = simpleAction;
        }

        ViewBag.Details = patient;
        ViewBag.patients = patients;

        Dictionary<double, string> color = new Dictionary<double, string>();

        color.Add(0,"rgb(0, 0, 0)");
        color.Add(1,"rgb(80,80,80)");
        color.Add(2,"rgb(110,110,110)");
        color.Add(3,"rgb(130,130,130)");
        color.Add(4,"rgb(140,140,140) ");
        color.Add(5,"rgb(150,150,150)" );
        color.Add(6,"rgb(170,170,170)" );
        color.Add(7,"rgb(190,190,190) ");
        color.Add(8,"rgb(210,210,210) ");
        color.Add(9,"rgb(230,230,230)" );
        color.Add(10,"rgb(255,255,255)" );

        ViewBag.loko = color;

        return View();
    }

    public IActionResult Change(string id)
    {
        HttpContext.Session.SetString("id_patient", id);
        return RedirectToAction("Index");
    }

    public IActionResult Repair(string id_tooth)
    {
        HttpContext.Session.SetString("id_tooth", id_tooth);
        return RedirectToAction("Index");
    }

    public IActionResult Priorite(string priorite, string budget)
    {
        HttpContext.Session.SetString("priorite", priorite);
        HttpContext.Session.SetString("budget", budget);
        return RedirectToAction("Index");
    }
}
