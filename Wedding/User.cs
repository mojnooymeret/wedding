namespace Wedding
{
   public class User
   {
      public string id { get; set; }
      public string message { get; set; }
      public string last { get; set; }
      public string fav_drink { get; set; }
      public string fav_food { get; set; }
      public string exeption { get; set; }
      public string music { get; set; }
      public string visit { get; set; }
      public string username { get; set; }
      public User(string id, string message, string last, string fav_drink, string fav_food, string exeption, string music, string visit, string username)
      {
         this.id = id;
         this.message = message;
         this.last = last;
         this.fav_drink = fav_drink;
         this.fav_food = fav_food;
         this.exeption = exeption;
         this.music = music;
         this.visit = visit;
         this.username = username;
      }
   }
}
