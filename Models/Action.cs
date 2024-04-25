using System;
using System.Collections.Generic;
using Npgsql;
using Utils.helper;

namespace Element
{
    public class Action
    {
        public string id_patient { get; set; }
        public string id_tooth { get; set; }
        public string action { get; set; }
        public double depense { get; set; }
        public double reste { get; set; }
        public double condition { get; set; }

        public Action()
        {
        }

        public Action(string idPatient, string idTooth, string actionName, double depense, double reste, double condition)
        {
            this.id_patient = idPatient;
            this.id_tooth = idTooth;
            this.action = actionName;
            this.depense = depense;
            this.reste = reste;
            this.condition = condition;
        }

        public static List<Action> Proposition(PatientTooth tooth, double budget)
        {
            PatientTooth nify = tooth;
            double reste = budget;
            double depense = 0;

            List<Action> Results = new List<Action>();

            List<Func<bool>> conditions = new List<Func<bool>>
            {
                () => nify.condition == 0 && reste - nify.tooth.replacement_price >= 0,
                () => nify.condition >= 1 && nify.condition <= 3 && reste - nify.tooth.removal_price>= 0,
                () => nify.condition >= 4 && nify.condition <= 6 && reste - nify.tooth.repair_price >= 0,
                () => nify.condition >= 7 && nify.condition <= 9 && reste - nify.tooth.cleaning_price >= 0
            };

            while (nify.condition != 10)
            {
                bool conditionMet = false;

                foreach (var condition in conditions)
                {
                    if (condition())
                    {
                        conditionMet = true;
                        ExecuteAction();
                        break;
                    }
                }

                if (!conditionMet)
                {
                    break;
                }
            }

            return Results;

            void ExecuteAction()
            {
                switch (nify.condition)
                {
                    case 0:
                        reste -= nify.tooth.replacement_price;
                        nify.condition = 10;
                        Results.Add(new Action(nify.id_patient, nify.id_tooth, "remplacer", nify.tooth.replacement_price, reste, nify.condition));
                        break;

                    case int conditionValue when conditionValue >= 1 && conditionValue <= 3:
                        reste -= nify.tooth.removal_price ;
                        double nb = nify.condition;
                        nify.condition += 1;
                        Results.Add(new Action(nify.id_patient, nify.id_tooth, "Grand reparation", nify.tooth.removal_price , reste, nify.condition));
                        break;

                    case int conditionValue when conditionValue >= 4 && conditionValue <= 6:
                        reste -= nify.tooth.repair_price;
                        nify.condition += 1;
                        Results.Add(new Action(nify.id_patient, nify.id_tooth, "Reparer", nify.tooth.repair_price, reste, nify.condition));
                        break;

                    case int conditionValue when conditionValue >= 7 && conditionValue <= 9:
                        reste -= nify.tooth.cleaning_price;
                        nify.condition += 1;
                        Results.Add(new Action(nify.id_patient, nify.id_tooth, "nettoyage", nify.tooth.cleaning_price, reste, nify.condition));
                        break;

                    default:
                        break;
                }
            }
        }

        public static List<Action> ProcessPatientTeeth(List<PatientTooth> patientTeeth, double initialBudget)
        {
            List<Action> allActions = new List<Action>();
            double currentBudget = initialBudget;

            foreach (PatientTooth patientTooth in patientTeeth)
            {
                List<Action> toothActions = Action.Proposition(patientTooth, currentBudget);

                if (toothActions.Count > 0)
                {
                    allActions.AddRange(toothActions);
                    currentBudget = toothActions[toothActions.Count - 1].reste;
                }
            }

            return allActions;
        }

         public void Insert(NpgsqlConnection conn)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id_patient", this.id_patient },
                { "id_tooth", this.id_tooth },
                { "action", this.action },
                { "depense", this.depense },
                { "reste", this.reste },
                { "condition", this.condition }
            };

            DatabaseHelper.Insert(conn, "action_table", data);
        }

        public static List<Action> Select(NpgsqlConnection conn, string idPatient, string idTooth)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "id_patient", idPatient },
                { "id_tooth", idTooth }
            };

            return DatabaseHelper.Select<Action>(conn, "action_table", parameters);
        }
    }
}
