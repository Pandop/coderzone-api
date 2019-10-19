using CoderzoneGrapQLAPI.Models;
using CoderzoneGrapQLAPI.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoderzoneGrapQLAPI.Api.DbSeeds
{
	public static class DbSeeding
	{
		//public static void SeedDataContext(this CoderzoneApiDbContext coderzoneApiContext)
		public static void SeedDataContext(this CoderzoneApiDbContext coderzoneApiContext)
		{
			var user = new Programmer();
			//var hashedPwd = userManager.PasswordHasher.HashPassword(user, "12343#QWqqwe");
			//userManager.CreateAsync(user, "adasdas");
			// Seed the database the country table is empty
			if (coderzoneApiContext.Countries.Count() == 0) {
				var hasher = new PasswordHasher<Programmer>();
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
										PasswordHash =hasher.HashPassword(null,"user@123"),
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
										PasswordHash =hasher.HashPassword(null,"user@321"),
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
												Name ="Codebot Jinkin Deployment",
												ImageUrl ="https://codebots.com/site/img/icon-developing-a-continuous-learning-mindset.png",
												Description="Providing continuous learning opportunities for your employees, including opportunities around new technologies, practices and industry developments, strengthens your workforce to adapt quickly to a rapidly changing world",

											},
											new Project
											{
												Name ="Reactbot Feature Development",
												ImageUrl ="https://codebots.com/site/img/legacy-burdens_codebots-blog-thumbnail.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											},
											new Project
											{
												Name ="CSharpbot Selenium Testing",
												ImageUrl ="https://codebots.com/site/img/scaling-up-software-people-fit_codebots-blog-thumbnail.png",
												Description="The real value of doing market research became clear when we realised we had missed something",
											}
										},
										WorkExperiences = new List<WorkExperience>()
										{
											new WorkExperience
											{
												Title="Delivering Codebots features",
												Description="For the past several weeks, our designers have been exploring the different directions we could take for the Codebots brand.",
												StartDate = new DateTime(2019, 4, 12),
												EndDate = new DateTime(2019, 6, 24)
											},
											new WorkExperience
											{
												Title="Delivering Reactbot Selenium Testing",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 8, 7),
												EndDate = new DateTime(2019, 9, 3)
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
											new Qualification{ Title="BSc of computer science", StartDate=new DateTime(2017,11,7),EndDate=new DateTime(2019,10,7), Description="Completed software development"},
											new Qualification{Title="Diploma of Agile Management", StartDate=new DateTime(2017,8,7),EndDate=new DateTime(2018,9,7), Description="Completed project management in software development in agile environment"},
											new Qualification{Title="Advanced Database Design", StartDate=new DateTime(2019,7,7),EndDate=new DateTime(2019,10,5), Description="Completed a certificate of database design using MYSQL"},
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
							},
							new State{
								Name ="NSW",
								PostCode ="3000",
								Programmer = new List<Programmer>(){
									new Programmer
									{
										Email = "dd.lutio@kel.com",
										UserName = "fm-luti@2003",
										PasswordHash =hasher.HashPassword(null,"user@121"),
										Profile = new Profile
										{
											FirstName = "Fredie",
											LastName = "Luti",
											Avatar = "https://img.cinemablend.com/filter:scale/quill/d/e/6/c/9/6/de6c96f1e9871aef148dbc51fb9a5bc90ff25314.jpg?mw=300",
											Bio = "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.",
											City = "Brisbane",
											Street = "Tahutimase Street",
											Number = 112,
											CreatedAt = DateTime.Now,
											UpdatedAt = DateTime.Now,
											DatePublished = new DateTime(2019, 3, 27),
										},
										Projects = new List<Project>()
										{
											new Project
											{
												Name ="Targeted Innovation",
												ImageUrl ="https://xbsoftware.com/wp-content/uploads/2018/03/why-software-projects-can-take-tonger-than-planned-banner.png",
												Description="Lorem Ipsum is simply dummy text of the printing and typesetting industry.",

											},
											new Project
											{
												Name ="Data Driven Presentation",
												ImageUrl ="https://www.projectmanager.com/wp-content/uploads/2015/08/award-winning-project-management-software.png",
												Description="There are many variations of passages of Lorem Ipsum available",
											},
											new Project
											{
												Name ="Enterprise Integration and Coordination",
												ImageUrl ="https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTsMIkTmxc_a_L3gcBFVBZfKX8Lacyk0fegAbvXZq1LFs-frj6h",
												Description="The real value of doing market research became clear when we realised we had missed something",
											}
										},
										WorkExperiences = new List<WorkExperience>()
										{
											new WorkExperience
											{
												Title="Enterprise Integration and Coordination",
												Description="For the past several weeks, our designers have been exploring the different directions we could take for the Codebots brand.",
												StartDate = new DateTime(2019, 4, 12),
												EndDate = new DateTime(2019, 6, 24)
											},
											new WorkExperience
											{
												Title="Data Driven Presentation",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 8, 7),
												EndDate = new DateTime(2019, 9, 3)
											},
											new WorkExperience
											{
												Title="Targeted Innovation",
												Description="To test our favourite two logo designs, we surveyed more than 100 designers, developers, business owners, and tech enthusiasts to get their help.",
												StartDate = new DateTime(2019, 6, 18),
												EndDate = new DateTime(2019, 10, 7)
											}
										},
										Qualifications = new List<Qualification>()
										{
											new Qualification{ Title="BSc of computer science", StartDate=new DateTime(2017,11,7),EndDate=new DateTime(2019,10,7), Description="Completed software development"},
											new Qualification{Title="Diploma of Agile Management", StartDate=new DateTime(2017,8,7),EndDate=new DateTime(2018,9,7), Description="Completed project management in software development in agile environment"},
											new Qualification{Title="Advanced Database Design", StartDate=new DateTime(2019,7,7),EndDate=new DateTime(2019,10,5), Description="Completed a certificate of database design using MYSQL"},
										},
										Skills = new List<Skill>()
										{
											new Skill{Name="Technical design"},
											new Skill{Name="Cross-tier components implementation"},
											new Skill{Name="Quality assurance"},
											new Skill{Name="Capacity and scalability planning"},
											new Skill{Name="Optimising and performance tuning"}
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
