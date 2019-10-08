using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.DbSeeds
{
	public static class DbSeeding
	{
		//public static void SeedDataContext(this CoderzoneApiDbContext coderzoneApiContext)
		public static void SeedDataContext(this CoderzoneApiDbContext coderzoneApiContext)
		{
			// Seed the database the country table is empty
			if (coderzoneApiContext.Countries.Count() == 0) { 
				var countries = new List<Country>() {
					new Country()
					{
						Name = "Australia",
						States= new List<State>()
						{
							new State
							{
								Name ="QLD",
								PostCode ="4000",
								Programmer= new List<Programmer>()
								{
									new Programmer
									{
										Email = "s.kel@kel.com",
										UserName = "amenemope@2000",
										Profile = new Profile
										{
											FirstName = "Kelei",
											LastName = "Garwech",
											Avatar = "https://i0.wp.com/thedisinsider.com/wp-content/uploads/2019/10/avatar-3.jpg?resize=750%2C430&ssl=1",
											Bio = "I’m a software engineer in Santa Barbara, CA with a passion for computer science, electrical engineering and embedded systems technology.",
											City = "Brisbane",
											Street = "Tehuti Street",
											Number = 12,
											CreatedAt = DateTime.Now,
											UpdatedAt = DateTime.Now,
											DatePublished = new DateTime(2019, 10, 7),											
										},
										Projects = new List<Project>()
										{
											new Project
											{
												Name ="Codebot Development",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The Codebots experiment is one of community-driven change and disruption. We're designing this awesome platform for you, so please keep hitting us up with your ideas and opinions",

											},
											new Project
											{
												Name ="Reactbot Development",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											},
											new Project
											{
												Name ="CSharpbot Testing",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											}
										},
										WorkExperiences = new List<WorkExperience>()
										{
											new WorkExperience
											{
												Title="Delivering Codebots",
												Description="For the past several weeks, our designers have been exploring the different directions we could take for the Codebots brand.",
												StartDate = new DateTime(2019, 6, 7),
												EndDate = new DateTime(2019, 10, 7)
											},
											new WorkExperience
											{
												Title="Delivering Reactbot",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 7, 7),
												EndDate = new DateTime(2019, 10, 7)
											},
											new WorkExperience
											{
												Title="Delivering CSharpbot Testings",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 6, 18),
												EndDate = new DateTime(2019, 10, 7)
											}
										},
										Qualifications = new List<Qualification>()
										{
											new Qualification{ Title="BSc of computer science", StartDate=new DateTime(2016,3,7),EndDate=new DateTime(2019,10,7), Description="Completed software development"},
											new Qualification{Title="Diploma of Project Management", StartDate=new DateTime(2018,3,7),EndDate=new DateTime(2018,9,7), Description="Completed project management in software development in agile environment"},
											new Qualification{Title="Certificate of Database Design", StartDate=new DateTime(2019,7,7),EndDate=new DateTime(2019,10,5), Description="Completed a certificate of database design using MYSQL"},
										},
										Skills = new List<Skill>()
										{
											new Skill{Name="Technical design"},
											new Skill{Name="Cross-tier components implementation"},
											new Skill{Name="Quality assurance"},
											new Skill{Name="Capacity and scalability planning"},
											new Skill{Name="Optimising and performance tuning"},
											new Skill{Name="Strong analytical skills"},
											new Skill{Name="Document management"},
											new Skill{Name="Agile/Scrum"},
											new Skill{Name="Team player"},
											new Skill{Name="Testing"},
										}
									},
									new Programmer
									{
										Email = "d.okelo@kel.com",
										UserName = "amenpaleguangee@2000",
										Profile = new Profile
										{
											FirstName = "Okello",
											LastName = "Duany",
											Avatar = "https://i0.wp.com/thedisinsider.com/wp-content/uploads/2019/10/avatar-3.jpg?resize=750%2C430&ssl=1",
											Bio = "I’m a software engineer in Santa Barbara, CA with a passion for computer science, electrical engineering and embedded systems technology.",
											City = "Brisbane",
											Street = "Tehuti Street",
											Number = 12,
											CreatedAt = DateTime.Now,
											UpdatedAt = DateTime.Now,
											DatePublished = new DateTime(2019, 10, 7),
										},
										Projects = new List<Project>()
										{
											new Project
											{
												Name ="Codebot Development",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The Codebots experiment is one of community-driven change and disruption. We're designing this awesome platform for you, so please keep hitting us up with your ideas and opinions",

											},
											new Project
											{
												Name ="Reactbot Development",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											},
											new Project
											{
												Name ="CSharpbot Testing",
												ImageUrl ="https://codebots.com/site/img/20170327-codebots-logo-01.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											}
										},
										WorkExperiences = new List<WorkExperience>()
										{
											new WorkExperience
											{
												Title="Delivering Codebots",
												Description="For the past several weeks, our designers have been exploring the different directions we could take for the Codebots brand.",
												StartDate = new DateTime(2019, 6, 7),
												EndDate = new DateTime(2019, 10, 7)
											},
											new WorkExperience
											{
												Title="Delivering Reactbot",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 7, 7),
												EndDate = new DateTime(2019, 10, 7)
											},
											new WorkExperience
											{
												Title="Delivering CSharpbot Testings",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 6, 18),
												EndDate = new DateTime(2019, 10, 7)
											}
										},
										Qualifications = new List<Qualification>()
										{
											new Qualification{ Title="BSc of computer science", StartDate=new DateTime(2016,3,7),EndDate=new DateTime(2019,10,7), Description="Completed software development"},
											new Qualification{Title="Diploma of Project Management", StartDate=new DateTime(2018,3,7),EndDate=new DateTime(2018,9,7), Description="Completed project management in software development in agile environment"},
											new Qualification{Title="Certificate of Database Design", StartDate=new DateTime(2019,7,7),EndDate=new DateTime(2019,10,5), Description="Completed a certificate of database design using MYSQL"},
										},
										Skills = new List<Skill>()
										{
											new Skill{Name="Technical design"},
											new Skill{Name="Cross-tier components implementation"},
											new Skill{Name="Quality assurance"},
											new Skill{Name="Capacity and scalability planning"},
											new Skill{Name="Optimising and performance tuning"},
											new Skill{Name="Strong analytical skills"},
											new Skill{Name="Document management"},
											new Skill{Name="Agile/Scrum"},
											new Skill{Name="Team player"},
											new Skill{Name="Testing"},
										}
									}
								}
							}
						}
					}
				};

				// Save countries to the Database
				coderzoneApiContext.Countries.AddRange(countries);
				coderzoneApiContext.SaveChanges();
			}
		}
	}
}
