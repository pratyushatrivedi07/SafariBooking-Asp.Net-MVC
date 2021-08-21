using Jungle.Entities;
using Jungle.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jungle.Repos
{
   public class IdentityProofRepository : IRepository<IdentityProof>
    {
        private MydbContext context;

        public IdentityProofRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(IdentityProof entity)
        {
            try
            {
                var employee = context.Tourist.FirstOrDefault(e => e.Id == entity.IdentityId);
                if (employee != null)
                {
                    throw new JungleException("identity already exist");
                }
                context.IdentityProof.Add(entity);
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

        public IdentityProof Get(object id)
        {
            try
            {


                IdentityProof identity = context.IdentityProof.Find(id);
                return identity;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<IdentityProof> GetAll()
        {
            try
            {
                return context.IdentityProof.ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(IdentityProof entity)
        {
            try
            {
                context.IdentityProof.Remove(entity);
                int Isdeleted = context.SaveChanges();
                if (Isdeleted > 0)
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

        public bool Update(IdentityProof entity)
        {
            try
            {

                context.IdentityProof.Update(entity);
                int IsUpdated = context.SaveChanges();
                if (IsUpdated > 0)
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
