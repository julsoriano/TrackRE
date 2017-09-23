using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.IO;
//using CITT.SemanticLogging;

namespace TrackRE.Models
{
    //public class SampleData : DropCreateDatabaseAlways<ApplicationDbContext>
    //public class SampleData : CreateDatabaseIfNotExists<ApplicationDbContext>
    public class SampleData : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    /*
     * An implementation of IDatabaseInitializer<TContext>that will delete, recreate, and optionally re-seed the database with data 
     * only if the model has changed since the database was created. 
     * This is achieved by writing a hash of the store model to the database when it is created and then comparing that hash with one generated from the current model. 
     * To seed the database, create a derived class and override the Seed method.
     * 
     * DropCreateDatabaseIfModelChanges<TContext>.Seed Method: A method that should be overridden to actually add data to the context for seeding. The default implementation does nothing.
     * protected virtual void Seed(TContext context)
     * 
     * Notes: 
     * 1. protected :A protected member is accessible within its class and by derived class instances only if the access occurs through the derived class type (See http://msdn.microsoft.com/en-us/library/bcd5672a.aspx)
     * 2. virtual: The virtual keyword is used to modify a method, property, indexer, or event declaration and allow for it to be overridden in a derived class
    */
    {
        protected override void Seed(ApplicationDbContext context)
        {
            SeedData(context);
        }

        public static void SeedData(ApplicationDbContext context)
        {
            /*
            new List<PropertyType>
            {
                new PropertyType { Name = "Condo", Description ="Condominium" },
                new PropertyType { Name = "TownHouse", Description ="Town House" },
                new PropertyType { Name = "House", Description ="House" },
                new PropertyType { Name = "Lot", Description ="Vacant Lot" },
                new PropertyType { Name = "HouseandLot", Description ="House and Lot"},
                new PropertyType { Name = "ParkingSpace", Description ="Parking Space" }                        
            }.ForEach(a => context.PropertyTypes.AddOrUpdate(a));
            context.SaveChanges();
            */

            context.PropertyTypes.AddOrUpdate(
                a => a.Name,
                new PropertyType { Name = "Condo", Description = "Condominium" },
                new PropertyType { Name = "TownHouse", Description = "Town House" },
                new PropertyType { Name = "House", Description = "House Excluding Lot" },
                new PropertyType { Name = "Lot", Description = "Vacant Lot" },
                new PropertyType { Name = "HouseandLot", Description = "House and Lot" },
                new PropertyType { Name = "ParkingSpace", Description = "Parking Space" }
            );
            context.SaveChanges();
                        
            //var owners = new List<Owner>
            context.Owners.AddOrUpdate(
                a => a.Name,             
                new Owner { Name = "Julito Soriano" },
                new Owner { Name = "Edilberto Jusay" }
            );
            context.SaveChanges();

            //var communities = new List<Community>
            context.Communities.AddOrUpdate(
                a => a.Name,            
                new Community { Name = "Two Serendra", Address = "11th Av., BGC, Taguig City, NCR" },
                new Community { Name = "Mezza I", Address = "Aurora Av., corner G. Araneta Av., Quezon City, NCR" },
                new Community { Name = "Treveia", Address = "Sta. Rosa, Laguna" }
            );            
            context.SaveChanges();
            
            /* new List<PropertyRE>
            {
                new PropertyRE { Description = "Two Serendra: Unit 510D", Location = "510D", Price = 7000000, 
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "Studio",
                    Community = communities.Single(g => g.Name == "Two Serendra"), 
                    Owner = owners.Single(g => g.Name == "Julito Soriano")},                    
                new PropertyRE { Description = "Mezza I: Unit T2-3309", Location = "3309 Tower 2", Price = 1600000, 
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "1BR",
                    Community = communities.Single(g => g.Name == "Mezza I"), 
                    Owner = owners.Single(g => g.Name == "Julito Soriano")},
                new PropertyRE { Description = "Mezza I: Unit T1-2306C", Location = "2306C Tower 1", Price = 1400000, 
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "1BR",
                    Community = communities.Single(g => g.Name == "Mezza I"), 
                    Owner = owners.Single(g => g.Name == "Edilberto Jusay")},
                new PropertyRE { Description = "Area: 240 sq. m.", Location = "Lot 17, Block 19, Phase 21", Price = 6000000, 
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Lot"),
                    PropertyTypeSub = "Vacant Lot",
                    Community = communities.Single(g => g.Name == "Treveia"), 
                    Owner = owners.Single(g => g.Name == "Edilberto Jusay")},
                new PropertyRE { Description = "Two Serendra: Parking B1-514", Location = "Dolce", Price = 1000000, 
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "ParkingSpace"),
                    PropertyTypeSub = "Parking Space",
                    Community = communities.Single(g => g.Name == "Two Serendra"), 
                    Owner = owners.Single(g => g.Name == "Julito Soriano")},                    
            }.ForEach(a => context.Properties.AddOrUpdate(a));
            context.SaveChanges();
            */

            context.Properties.AddOrUpdate(
                a => a.Description,
                new PropertyRE
                {
                    Description = "Two Serendra: Unit 510D",
                    Location = "510D",
                    Price = 7000000,
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "Studio",
                    Community = context.Communities.Single(g => g.Name == "Two Serendra"),
                    Owner = context.Owners.Single(g => g.Name == "Julito Soriano")
                },
                new PropertyRE
                {
                    Description = "Mezza I: Unit T2-3309",
                    Location = "3309 Tower 2",
                    Price = 1600000,
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "1BR",
                    Community = context.Communities.Single(g => g.Name == "Mezza I"),
                    Owner = context.Owners.Single(g => g.Name == "Julito Soriano")
                },
                new PropertyRE
                {
                    Description = "Mezza I: Unit T1-2306C",
                    Location = "2306C Tower 1",
                    Price = 1400000,
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Condo"),
                    PropertyTypeSub = "1BR",
                    Community = context.Communities.Single(g => g.Name == "Mezza I"),
                    Owner = context.Owners.Single(g => g.Name == "Edilberto Jusay")
                },
                new PropertyRE
                {
                    Description = "Area: 240 sq. m.",
                    Location = "Lot 17, Block 19, Phase 21",
                    Price = 6000000,
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "Lot"),
                    PropertyTypeSub = "Vacant Lot",
                    Community = context.Communities.Single(g => g.Name == "Treveia"),
                    Owner = context.Owners.Single(g => g.Name == "Edilberto Jusay")
                },
                new PropertyRE
                {
                    Description = "Two Serendra: Parking B1-514",
                    Location = "Dolce",
                    Price = 1000000,
                    PropertyType = context.PropertyTypes.Single(g => g.Name == "ParkingSpace"),
                    PropertyTypeSub = "Parking Space",
                    Community = context.Communities.Single(g => g.Name == "Two Serendra"),
                    Owner = context.Owners.Single(g => g.Name == "Julito Soriano")
                }
            );
            context.SaveChanges();
        }
    }
}