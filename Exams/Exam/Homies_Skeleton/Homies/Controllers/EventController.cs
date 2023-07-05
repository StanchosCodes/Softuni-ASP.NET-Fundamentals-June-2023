using Homies.Data;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Homies.Controllers
{
	[Authorize]
	public class EventController : Controller
	{
		private readonly HomiesDbContext context;

        public EventController(HomiesDbContext context)
        {
			this.context = context;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<EventViewModel> events = await this.context.Events
				.Select(e => new EventViewModel()
				{
					Id = e.Id,
					Name = e.Name,
					Start = e.Start,
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName
				})
				.ToListAsync();

			return View(events);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			List<TypeViewModel> types = await this.context.Types
				.Select(t => new TypeViewModel()
				{
					Id = t.Id,
					Name = t.Name
				})
				.ToListAsync();

			AddEditViewModel addModel = new AddEditViewModel()
			{
				Types = types
			};

			return View(addModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddEditViewModel addModel)
		{
			if (!ModelState.IsValid)
			{
				return View(addModel);
			}

			try
			{
				var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

				if (userId == null)
				{
					return Unauthorized();
				}

				Event newEvent = new Event()
				{
					Name = addModel.Name,
					Description = addModel.Description,
					OrganiserId = userId,
					CreatedOn = DateTime.Parse(DateTime.UtcNow.ToString("dd/MM/yyyy H:mm")),
					Start = DateTime.Parse(addModel.Start.ToString("dd/MM/yyyy H:mm")),
					End = DateTime.Parse(addModel.End.ToString("dd/MM/yyyy H:mm")),
					TypeId = addModel.TypeId
				};

				await this.context.Events.AddAsync(newEvent);
				await this.context.SaveChangesAsync();

				return RedirectToAction("All", "Event");
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Failed to add event!");

				return View(addModel);
			}
		}

		public async Task<IActionResult> Joined()
		{
			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			List<EventViewModel> events = await this.context.EventsParticipants
				.Where(ep => ep.HelperId == userId)
				.Select(ep => new EventViewModel()
				{
					Id = ep.Event.Id,
					Name = ep.Event.Name,
					Start = ep.Event.Start,
					Type = ep.Event.Type.Name,
					Organiser = ep.Event.Organiser.UserName
				})
				.ToListAsync();

			return View(events);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			List<TypeViewModel> types = await this.context.Types
				.Select(t => new TypeViewModel()
				{
					Id = t.Id,
					Name = t.Name
				})
				.ToListAsync();

			Event? eventToEdit = await this.context.Events.FindAsync(id);

			if (eventToEdit == null)
			{
				return BadRequest();
			}

			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != eventToEdit.OrganiserId)
			{
				return RedirectToAction("All", "Event");
			}

			AddEditViewModel eventModel = new AddEditViewModel()
			{
				Name = eventToEdit.Name,
				Description = eventToEdit.Description,
				Start = eventToEdit.Start,
				End = eventToEdit.End,
				TypeId = eventToEdit.TypeId,
				Types = types
			};

			return View(eventModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, AddEditViewModel editedEvent)
		{
			if (!ModelState.IsValid)
			{
				return View(editedEvent);
			}

			Event? eventToEdit = await this.context.Events.FindAsync(id);

			if (eventToEdit == null)
			{
				return BadRequest();
			}

			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != eventToEdit.OrganiserId)
			{
				return Unauthorized();
			}

			eventToEdit.Name = editedEvent.Name;
			eventToEdit.Description = editedEvent.Description;
			eventToEdit.Start = editedEvent.Start;
			eventToEdit.End = editedEvent.End;
			eventToEdit.TypeId = editedEvent.TypeId;

			await this.context.SaveChangesAsync();

			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Join(int id)
		{
			EventViewModel? eventToJoin = await this.context.Events
				.Where(e => e.Id == id)
				.Select(e => new EventViewModel()
				{
					Id = e.Id,
					Name = e.Name,
					Start = e.Start,
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName
				})
				.FirstOrDefaultAsync();

			if (eventToJoin == null)
			{
				return RedirectToAction("All", "Event");
			}

			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized();
			}

			bool isContained = await this.context.EventsParticipants.AnyAsync(ep => ep.EventId == id && ep.HelperId == userId);

			if (isContained)
			{
				return RedirectToAction("All", "Event");
			}

			EventParticipant eventParticipant = new EventParticipant()
			{
				EventId = eventToJoin.Id,
				HelperId = userId
			};

			await this.context.EventsParticipants.AddAsync(eventParticipant);
			await this.context.SaveChangesAsync();

			return RedirectToAction("Joined", "Event");
		}

		public async Task<IActionResult> Leave(int id)
		{
			EventViewModel? eventToLeave = await this.context.Events
				.Where(e => e.Id == id)
				.Select(e => new EventViewModel()
				{
					Id = e.Id,
					Name = e.Name,
					Start = e.Start,
					Type = e.Type.Name,
					Organiser = e.Organiser.UserName
				})
				.FirstOrDefaultAsync();

			if (eventToLeave == null)
			{
				return RedirectToAction("Joined", "Event");
			}

			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized();
			}

			EventParticipant? eventParticipant = await this.context.EventsParticipants
				.FirstOrDefaultAsync(ep => ep.EventId == id && ep.HelperId == userId);

			if (eventParticipant != null)
			{
				this.context.EventsParticipants.Remove(eventParticipant);
				await this.context.SaveChangesAsync();
			}

			return RedirectToAction("All", "Event");
		}

		public async Task<IActionResult> Details(int id)
		{
			DetailsViewModel? detailsModel = await this.context.Events
				.Where(e => e.Id == id)
				.Select(e => new DetailsViewModel()
				{
					Id = e.Id,
					Name = e.Name,
					Description = e.Description,
					Start = e.Start,
					End = e.End,
					Organiser = e.Organiser.UserName,
					CreatedOn = e.CreatedOn,
					Type = e.Type.Name
				})
				.FirstOrDefaultAsync();

			if (detailsModel == null)
			{
				return RedirectToAction("All", "Event");
			}

			return View(detailsModel);
		}
	}
}
