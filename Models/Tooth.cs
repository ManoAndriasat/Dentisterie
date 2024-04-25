using System;
using System.Collections.Generic;
using Npgsql;
using Utils.helper;

namespace Element
{
    public class Tooth
    {
    public string id_tooth { get; set; }
    private double _replacementPrice;
    private double _cleaningPrice;
    private double _repairPrice;
    private double _removalPrice;
    private int _beautyPriority;
    private int _healthPriority;


        public double replacement_price
        {
            get { return _replacementPrice; }
            set{
                if (value < 0){
                    throw new ArgumentException("Le prix de remplacement doit être positif.");
                }
                _replacementPrice = value;
            }
        }

        public double cleaning_price
        {
            get { return _cleaningPrice; }
            set{
                if (value < 0){
                    throw new ArgumentException("Le prix de nettoyage doit être positif.");
                }
                _cleaningPrice = value;
            }
        }

        public double repair_price
        {
            get { return _repairPrice; }
            set{
                if (value < 0){
                    throw new ArgumentException("Le prix de réparation doit être positif.");
                }
                _repairPrice = value;
            }
        }

        public double removal_price
        {
            get { return _removalPrice; }
            set{
                if (value < 0){
                    throw new ArgumentException("Le prix de retrait doit être positif.");
                }
                _removalPrice = value;
            }
        }

        public int beauty_priority
        {
            get { return _beautyPriority; }
            set{
                if (value < 0){
                    throw new ArgumentException("La priorité beauté doit être positive.");
                }
                _beautyPriority = value;
            }
        }

        public int health_priority
        {
            get { return _healthPriority; }
            set{
                if (value < 0){
                    throw new ArgumentException("La priorité santé doit être positive.");
                }
                _healthPriority = value;
            }
        }

        public Tooth()
        {
        }

        public Tooth(string idTooth, double replacementPrice, double cleaningPrice, double repairPrice, double removalPrice, int beautyPriority, int healthPriority)
        {
            id_tooth = idTooth;
            replacement_price = replacementPrice;
            cleaning_price = cleaningPrice;
            repair_price = repairPrice;
            removal_price = removalPrice;
            beauty_priority = beautyPriority;
            health_priority = healthPriority;
        }

        public static void Insert(NpgsqlConnection conn, Tooth tooth)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id_tooth", tooth.id_tooth },
                { "replacement_price", tooth.replacement_price },
                { "cleaning_price", tooth.cleaning_price },
                { "repair_price", tooth.repair_price },
                { "removal_price", tooth.removal_price },
                { "beauty_priority", tooth.beauty_priority },
                { "health_priority", tooth.health_priority }
            };

            DatabaseHelper.Insert(conn, "tooth", data);
        }

        public static List<Tooth> SelectAll(NpgsqlConnection conn){
            return DatabaseHelper.Select<Tooth>(conn, "tooth");
        }

        public static Tooth SelectById(NpgsqlConnection conn, string id_tooth)
        {
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "id_tooth", id_tooth }
            };
            List<Tooth> result = DatabaseHelper.Select<Tooth>(conn, "tooth", data);
            return result.FirstOrDefault();
        }
    }
}