using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using MovieStore.Domain;

namespace MovieStore.App
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var nhConfig = new Configuration().Configure();
            var sessionFactory = nhConfig.BuildSessionFactory();
            Console.WriteLine("NHibernate is now configured");

            var schemaExport = new SchemaExport(nhConfig);
            //let NHibernate create your database with the following statement 
            //it's easy for now, but usually not appropriate for your application to
            //recreate database tables each time
            schemaExport.Create(false, true);
            //use the following statement if you want the db.sql script so you can execute it yourself
            //schemaExport.SetOutputFile(@"db.sql").Execute(false, false, false);
            Console.WriteLine("Database schema created");
            
			
			
			using (ISession session = sessionFactory.OpenSession())
			{
			  using (session.BeginTransaction())
				{
					Genre g1 = new Genre{Name="Thriller"};
					session.SaveOrUpdate(g1);
					session.Transaction.Commit();
				}
			}
			
			Genre myGenre;
			using (ISession session = sessionFactory.OpenSession())
			{
				myGenre = session.Get<Genre>(1);
			}
			Console.WriteLine("Found a " + myGenre + " genre");
			
			using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    Movie m1 = new Movie { Name = "Old Boy", 
                        Description = "Korean movie", 
                        ReleaseYear = 2008, 
                        Genre = myGenre };
                    session.Save(m1);
                    session.Transaction.Commit();
                }
            }
			
			Movie myMovie;
            using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    myMovie = session.Get<Movie>(1);
                }
				Console.WriteLine("Founed a " + myMovie.Genre.Name + " movie: " + myMovie.Name);
            }
            
			using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    Genre g1 = new Genre { Name = "Drama" };
                    Movie m1 = new Movie
                    {
                        Name = "Loft",
                        Description = "Bart De Pauw did his best",
                        ReleaseYear = 2009,
                        Genre = myGenre
                    };
                    Movie m2 = new Movie
                    {
                        Name="127 Hours",
                        Description = "Time is ticking",
                        ReleaseYear = 2010,
                        Genre = g1
                    };
					// session.Save(g1); not needed because of the cascase="save-update" option in mapping file
                    session.Save(m1);
                    session.Save(m2);
                    session.Transaction.Commit();
                }
            }

			using (ISession session = sessionFactory.OpenSession())
            {
                using (session.BeginTransaction())
                {
                    IQuery query = session.CreateQuery("from Movie as m order by m.Name");
                    IList<Movie> movieList = query.List<Movie>();
                    foreach (Movie m in movieList)
                    {
                        Console.WriteLine("Found movie " + m.Name + ".");
                    }
                }
            }
			
			using (ISession session = sessionFactory.OpenSession())
			{
				using (session.BeginTransaction())
				{
					Movie m1 = session.Get<Movie>(2);
					Actor a1 = new Actor{FirstName="Koen",LastName="De Bouw",BirthDate=new DateTime(1964,9,30)};
					Actor a2 = new Actor{FirstName="Koen",LastName="De Graeve",BirthDate=new DateTime(1974,9,30)};
					m1.Actors.Add(a1);
					m1.Actors.Add(a2);
					session.SaveOrUpdate(m1);
					session.Transaction.Commit();
				}
			}
			
			using (ISession session = sessionFactory.OpenSession())
			{
				using (session.BeginTransaction())
				{
					Actor a1 = session.Get<Actor>(2);
					Console.WriteLine("Actor found: " + a1.FirstName + " " + a1.LastName);
					foreach (Movie m in a1.Movies)
					{
						Console.WriteLine("Movie: " + m.Name);
					}
					
				}
			}



			
			Console.ReadKey();
		}
	}
}
