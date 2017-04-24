using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class TaskDbInitializer : DropCreateDatabaseIfModelChanges<TaskDbContext>
    {
        protected override void Seed(TaskDbContext context)
        {
            base.Seed(context);

            var helper = new TTWebApiHelper();
            var symbolsList = helper.GetSymbols();
            var quotesList = new List<Quote>();
            foreach (var item in symbolsList)
            {
                quotesList.AddRange(helper.GetQuotes(item));
            }
            context.Symbols.AddRange(symbolsList);
            context.Quotes.AddRange(quotesList);
            context.SaveChanges();
        }
    }
}