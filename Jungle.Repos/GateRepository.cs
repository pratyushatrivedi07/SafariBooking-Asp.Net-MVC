using Jungle.Entities;
using Jungle.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jungle.Repos
{
    public class GateRepository : IGateRepository
    {
        private MydbContext context;

        public GateRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(Gate entity)
        {
            try
            {
                var dept = context.Gate.FirstOrDefault(e => e.Name.ToLower() == entity.Name.ToLower() && e.ParkId == entity.ParkId);
                if (dept != null)
                {
                    throw new JungleException("Gate already exists");
                }
                context.Gate.Add(entity);
                int recordsAffected = context.SaveChanges();
                if (recordsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public Gate Get(object id)
        {
            try
            {
                Gate gate = context.Gate.Find(id);
                return gate;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Gate> GetAll()
        {
            try
            {
                return context.Gate.Include(g=> g.Park).ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Gate entity)
        {
            try
            {
                context.Gate.Remove(entity);
                int recordAffected = context.SaveChanges();
                if (recordAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                throw new JungleException("Cannot delete as there as booking against this Gate");
            }
        }

        public IEnumerable<Gate> GetByPark(int parkId)
        {
            try
            {
                var v = context.Gate.Where(e => e.ParkId == parkId).Include(e => e.Park).ToList();
                return v;
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Update(Gate entity)
        {
            try
            {
                context.Gate.Update(entity);
                int recordAffected = context.SaveChanges();
                if (recordAffected > 0)
                {
                    return true;
                }
                return false;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }
    }
}
