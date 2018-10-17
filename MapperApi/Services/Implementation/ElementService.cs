using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapper_Api.Context;
using Mapper_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mapper_Api.Services
{
    public class ElementService : IElementService
    {
        private ZoneDB context;

        public ElementService(ZoneDB context)
        {
            this.context = context;
        }

        public async Task<Element> CreateElementAsync(Zone zone, Element element)
        {
            if (context.Zones.Any(z => z.ZoneID == zone.ZoneID))
            {
                try
                {
                    element.ZoneID = zone.ZoneID;
                    element.ElementId = Guid.NewGuid();
                    if (element.ElementType != null)
                    {
                        switch (element.ElementType)
                        {
                            case Element.ElementTypes.POINT:
                                context.Points.Add((Point)element);
                                await context.SaveChangesAsync();
                                return element;
                            case Element.ElementTypes.POLYGON:
                                context.Polygons.Add((Polygon)element);
                                await context.SaveChangesAsync();
                                return element;
                        }
                    }
                    throw new ArgumentException("Invalid element provided");
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Invalid element provided", e.Message);
                }
            }
            throw new ArgumentException("Invalid zone provided");
        }

        public async Task<Element> DeleteElementAsync(Element element)
        {
            if (context.Elements.Any(z => z.ElementId == element.ElementId))
            {
                element = await context.Elements.Where(z => z.ElementId == element.ElementId)
                        .FirstOrDefaultAsync();
                try
                {
                    context.Elements.Remove(element);
                    await context.SaveChangesAsync();
                    return element;
                }
                catch (Exception)
                {
                    throw new ArgumentException("Invalid element provided");
                }
            }
            throw new ArgumentException("Invalid element provided");
        }

        public async Task<Element> GetElementAsync(Element element)
        {
            try
            {
                element = await context.Elements
                    .Where(e => e.ElementId == element.ElementId)
                    .SingleOrDefaultAsync();
                if (element != null)
                {
                    return element;
                }
                throw new ArgumentException("Invalid element provided");
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid element provided");
            }
        }

        public async Task<List<Element>> GetElementsAsync(Zone zone)
        {
            try
            {
                if ((zone = await context.Zones
                .Where(z => z.ZoneID == zone.ZoneID)
                .Include(z => z.Elements).SingleOrDefaultAsync()) != null)
                {
                    return zone.Elements;
                }
                throw new ArgumentException("Invalid element provided");
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid element provided");
            }
        }

        public Task<List<Element>> GetElementsAsync(Zone zone, Element element)
        {
            try
            {
                if (element.ElementType != null)
                {
                    switch (element.ElementType)
                    {
                        case Element.ElementTypes.POINT:
                            return context.Points.Where(pt => pt.ZoneID == zone.ZoneID).Cast<Element>().ToListAsync();
                        case Element.ElementTypes.POLYGON:
                            return context.Polygons.Where(pt => pt.ZoneID == zone.ZoneID).Cast<Element>().ToListAsync();
                    }
                }
                throw new ArgumentException("Invalid parameters");
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid parameters");
            }
        }

        public async Task<Element> UpdateElementAsync(Element element)
        {
            try
            {
                Element oldElem;
                if ((oldElem = await context.Elements
                .Where(z => z.ElementId == element.ElementId).SingleOrDefaultAsync()) != null)
                {
                    if (element.ElementId != null)
                        oldElem.ElementId = element.ElementId;
                    if (element.ElementType != null)
                        oldElem.ElementType = element.ElementType;
                    if (element.Info != null)
                        oldElem.Info = element.Info;
                    if (element.GeoJson != null)
                        oldElem.GeoJson = element.GeoJson;

                    context.Update(oldElem);
                    await context.SaveChangesAsync();
                    return oldElem;
                }
                throw new ArgumentException("Invalid element provided");
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid element provided");
            }
        }
    }
}