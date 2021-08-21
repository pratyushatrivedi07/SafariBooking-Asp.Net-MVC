using Jungle.Entities;
using Jungle.Exceptions;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jungle.Repos
{
   public class PaymentRepository : IRepository<Payment>
    {
        private MydbContext context;

        public PaymentRepository(MydbContext context)
        {
            this.context = context;
        }
        public bool Add(Payment entity)
        {
            try
            {
           
                context.Payment.Add(entity);
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

        public Payment Get(object id)
        {
            try
            {


                Payment pay = context.Payment.Find(id);
                return pay;

            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            try
            {
                return context.Payment.ToList();
            }
            catch (SqlException ex)
            {

                throw new JungleException(ex.Message);
            }
        }

        public bool Remove(Payment entity)
        {
            try
            {
                context.Payment.Remove(entity);
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

        public bool Update(Payment entity)
        {
            try
            {

                context.Payment.Update(entity);
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
