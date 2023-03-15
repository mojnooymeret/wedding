using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Wedding
{
   public class Connect
   {
      public static SQLiteDataReader Query(string str)
      {
         SQLiteConnection SQLiteConnection = new SQLiteConnection("Data Source=|DataDirectory|wedding.db");
         SQLiteCommand SQLiteCommand = new SQLiteCommand(str, SQLiteConnection);
         try {
            SQLiteConnection.Open();
            SQLiteDataReader reader = SQLiteCommand.ExecuteReader();
            return reader;
         } catch { return null; }
      }

      public static void LoadUser(List<User> data)
      {
         try {
            data.Clear();
            SQLiteDataReader query = Query("select * from `User`;");
            if (query != null) {
               while (query.Read()) {
                  data.Add(new User(
                     query.GetValue(0).ToString(),
                     query.GetValue(1).ToString(),
                     query.GetValue(2).ToString(),
                     query.GetValue(3).ToString(),
                     query.GetValue(4).ToString(),
                     query.GetValue(5).ToString(),
                     query.GetValue(6).ToString(),
                     query.GetValue(7).ToString(),
                     query.GetValue(8).ToString()
                  ));
               }
            }
         } catch { }
         
      }
   }
}
