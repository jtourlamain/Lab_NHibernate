using System;
using Iesi.Collections.Generic;

namespace MovieStore.Domain
{
	public class Actor: Entity
	{
		public virtual string FirstName{get;set;}
		public virtual string LastName{get;set;}
		public virtual DateTime BirthDate {get;set;}
		public virtual ISet<Movie> Movies{get;set;}
		
		public Actor()
		{
			Movies = new HashedSet<Movie>();
		}
	}
}

