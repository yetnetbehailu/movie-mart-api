using System;
using Microsoft.EntityFrameworkCore;


namespace movie_mart_api
{
	public class MovieMartContext: DbContext
	{

        //Constructor
        public MovieMartContext (DbContextOptions options) : base(options){

		}
	}
}

