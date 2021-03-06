﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ManicureLand.Services
{
    public class DBAccess
    {
        private static string STRING_CONECTION = "Data Source=localhost\\SQLEXPRESS;Initial Catalog = ManicureLand;Trusted_Connection=true;";
        private SqlConnection conexion;

        public DBAccess()
        {
            conexion = new SqlConnection(STRING_CONECTION);
        }

        public SqlDataReader BuscarRegistro(string query)
        {
            SqlCommand command = new SqlCommand();
            SqlDataReader sqlDataReader;
            conexion.Open();
            command.Connection = conexion;
            
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            sqlDataReader = command.ExecuteReader();
            return sqlDataReader;
        }

        public bool DeshabilitarRegistro(string tabla, string nombreId, int id)
        {
            try
            {
                SqlCommand command = new SqlCommand();
                String query = "UPDATE " + tabla + " SET estado = 0 where " + nombreId + " = " + id.ToString() + " and estado = 1";
                int numDatos;
                conexion.Open();
                command.Connection = conexion;
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                numDatos = command.ExecuteNonQuery();
                if (numDatos < 1)
                {
                    return false;
                }
                return true;
            }
            catch {
                return false;
            }
        }

        public bool ModificarDatos(string tabla, List<SqlParameter> parametros, string filtro)
        {
            try
            {
                if (parametros.Count < 1 || tabla.Length < 1)
                {
                    return false;
                }
                SqlCommand command = new SqlCommand();
                bool respuesta = false;
                int numDatos;
                string query = "";
                string param = "";

                conexion.Open();
                command.Connection = conexion;

                query = "UPDATE " + tabla + " SET ";
                for (int i = 0; i < parametros.Count; i++)
                {
                    query = query + parametros[i].ParameterName;                    
                    param = "@param" + (i + 1);
                    query = query + " = " + param;
                    if (i < parametros.Count - 1)
                    {
                        query = query + ", ";
                    }
                    else
                    {
                        query = query + " WHERE " + filtro;
                    }
                    command.Parameters.AddWithValue(param, parametros[i].Value);
                }

                command.CommandType = CommandType.Text;
                command.CommandText = query;
                numDatos = command.ExecuteNonQuery();
                if (numDatos > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
                conexion.Close();
                return respuesta;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public bool InsertarDatos(string tabla, List<SqlParameter> parametros)
        {
            try
            {
                if (parametros.Count < 1 || tabla.Length < 1)
                {
                    return false;
                }
                SqlCommand command = new SqlCommand();
                bool respuesta = false;
                int numDatos;
                string query = "";
                string paramX = "";
                string param = "";

                conexion.Open();
                command.Connection = conexion;

                query = "INSERT INTO " + tabla + "(";
                for (int i = 0; i < parametros.Count; i++)
                {
                    query = query + parametros[i].ParameterName;
                    param = "@param" + (i + 1);
                    paramX = paramX + param;
                    if (i < parametros.Count - 1)
                    {
                        query = query + ", ";
                        paramX = paramX + ", ";
                    }
                    else
                    {
                        query = query + ") VALUES(" + paramX + ")";
                    }
                    command.Parameters.AddWithValue(param, parametros[i].Value);
                }

                command.CommandType = CommandType.Text;
                command.CommandText = query;
                numDatos = command.ExecuteNonQuery();
                if (numDatos > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
                }
                conexion.Close();
                return respuesta;                
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}