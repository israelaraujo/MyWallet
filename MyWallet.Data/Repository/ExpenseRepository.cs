﻿using MyWallet.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;

namespace MyWallet.Data.Repository
{
    public class ExpenseRepository
    {
        private MyWalletDBContext _context;

        public ExpenseRepository(MyWalletDBContext context)
        {
            _context = context;
        }

        public void Add(Expense expense)
        {
            _context.Expense.Add(expense);
        }

        public void Add(IEnumerable<Expense> expenses)
        {
            _context.Expense.AddRange(expenses);
        }

        public void Update(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Modified;
        }

        public void Delete(Expense expense)
        {
            _context.Entry(expense).State = EntityState.Deleted;
        }

        public void Delete(string id)
        {
            _context.Entry(new Expense { Id = id }).State = EntityState.Deleted;
        }

        public Expense GetById(string id)
        {
            return _context.Expense.Find(id);
        }

        public IEnumerable<Expense> GetAllByContextId(string contextId)
        {
            using (var connection = new SqlConnection(_context.Database.Connection.ConnectionString))
            {
                //connection.Open();

                //var sqlText = @"SELECT 
                //                    e.Id, 
	               //                 e.Description, 
	               //                 e.Value, 
	               //                 e.Date, 
	               //                 e.IsPaid, 
	               //                 e.Observation, 
	               //                 b.Name BankAccount,
                //                    c.Name Category
                //                FROM
                //                    Expense e
                //                INNER JOIN Category c ON c.Id = e.CategoryId
                //                INNER JOIN BankAccount b ON b.Id = e.BankAccountId
                //                WHERE
                //                    e.ContextId = @ContextId";

                //var command = new SqlCommand(sqlText, connection);
                //command.Parameters.AddWithValue("ContextId", contextId);

                //var result = command.ExecuteReader();

                var expenses = new List<Expense>();
                //while (result.Read())
                //{
                //    var expense = new Expense();
                //    expense.Id = int.Parse(result["Id"].ToString());
                //    expense.Description = result["Description"].ToString();
                //    expense.Value = decimal.Parse(result["Value"].ToString());
                //    expense.Date = DateTime.Parse(result["Date"].ToString());
                //    expense.IsPaid = bool.Parse(result["IsPaid"].ToString());
                //    expense.Observation = result["Observation"].ToString();
                //    expense.BankAccount = new BankAccount { Name = result["BankAccount"].ToString() };
                //    expense.Category = new Category { Name = result["Category"].ToString() };

                //    expenses.Add(expense);
                //}
                return expenses;
            }
        }

    }
}
