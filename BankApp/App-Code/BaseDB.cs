using System;
using System.Data.Entity;
using System.Reflection;

namespace DatabaseStruct
{
    public class BaseDB
    {
        private string DB_Context_String  = "default";
        private string DB_Table_String    = "default";
        private string DB_Table_NameSpace = "default";

        public virtual int ID { get; set; }

        //JDR: Get DB credentials
        protected BaseDB(string DB, string Table)
        {
            this.ID = 0;

            this.DB_Context_String = DB;
            this.DB_Table_NameSpace = Table;
            this.DB_Table_String = Table.Split('.')[2];
        }

        //JDR: Static method to initialize object
        public dynamic New()
        {
            Type Object_Type = Assembly.GetExecutingAssembly().GetType(this.DB_Table_NameSpace);

            dynamic Object_Class = Activator.CreateInstance(Object_Type);
            dynamic Object = Convert.ChangeType(Object_Class, Object_Type);

            return Object;
        }

        //JDR: Get DB Object dynamically
        private dynamic GetDbContext()
        {
            Type DB_Type = Assembly.GetExecutingAssembly().GetType(this.DB_Context_String);
            DbContext DB_Context = (DbContext)Activator.CreateInstance(DB_Type);

            dynamic Database = Convert.ChangeType(DB_Context, DB_Type);

            return Database;
        }

        //JDR: Get tABLE DbSet Object dynamically
        private DbSet GetTableDbSet(dynamic Database)
        {
            DbSet Table = Database.GetType().GetProperty(this.DB_Table_String).GetValue(Database, null);

            return Table;
        }

        //JDR:Get record by ID
        public dynamic GetByID(int ID)
        {
            var db = this.GetDbContext();
            DbSet Table = GetTableDbSet(db);

            var Record = Table.Find(ID);

            db.Dispose();

            return Record;
        }
        
        //JDR: Add Object to DB
        public dynamic Add()
        {
            //JDR: Dont add, already in DB
            if (this.ID > 0) return false;

            var db = this.GetDbContext();

            try
            {
                DbSet Table = GetTableDbSet(db);
                Table.Add(this);
                db.SaveChanges();

                return this.ID;
            }
            catch (Exception Exception)
            {
                //JDR: Exception can be logged here
                Console.WriteLine("***EROR_CATCH*** - An error occurred: '{0}'", Exception);

                return false;
            }
            finally
            {
                db.Dispose();
            }
        }

        //JDR: Save Object to DB
        public bool Save()
        {
            if (this.ID == 0)
            {
                this.Add();
                return (this.ID >= 0) ? true : false;
            }

            var db = this.GetDbContext();

            try
            {
                db.Entry(this).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception Exception)
            {
                //JDR: Exception can be logged here
                Console.WriteLine("***EROR_CATCH*** - An error occurred: '{0}'", Exception);

                return false;
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}