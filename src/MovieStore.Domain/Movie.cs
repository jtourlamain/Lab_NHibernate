using System;
using Iesi.Collections.Generic;

namespace MovieStore.Domain
{
	public class Movie: Entity
	{
		public virtual string Name{get;set;}
		public virtual string Description{get;set;}
		public virtual int ReleaseYear{get;set;}
		public virtual Genre Genre{get;set;}
		public virtual ISet<Actor> Actors{get;set;}
		
		public Movie()
		{
			Actors = new HashedSet<Actor>();
		}
	}
}

