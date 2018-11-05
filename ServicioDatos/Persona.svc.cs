using ServicioDatos.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace ServicioDatos
{
    
    //[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Persona : IPersona
    {
        private censoEntities db = new censoEntities();

        public RespuestaDTO Personas()
        {
            RespuestaDTO r = new RespuestaDTO();
            List<InfoDTO> listado = new List<InfoDTO>();

            try
            {
                var personas = db.persona.Where(item => item.estado == 1).ToList();

                foreach (var registro in personas)
                {
                    InfoDTO dto = new InfoDTO();
                    dto.Id = registro.id_persona;
                    dto.Nombre = registro.nombre;
                    dto.Apellido = registro.apellido;
                    dto.Edad = registro.edad.ToString();
                    dto.Estado = registro.estado.ToString();

                    listado.Add(dto);
                }

                r.Listado = listado;
            }
            catch (Exception ex)
            {
                r.Error = ex.InnerException.Message;
            }

            return r;
        }

        public RespuestaDTO Personas2(string id)
        {
            RespuestaDTO r = new RespuestaDTO();
            List<InfoDTO> listado = new List<InfoDTO>();

            int idPersona = 0;
            int.TryParse(id, out idPersona);

            if(idPersona == 0)
            {
                r.Error = "El valor ingresado en id no es un dato numérico";
                return r;
            }


            try
            {
                var personas = db.persona.Where(item => item.id_persona == idPersona && item.estado == 1).ToList();

                if (personas.Count == 0)
                    r.Error = "No existe registro de persona con id " + idPersona;
                else
                {
                    foreach (var registro in personas)
                    {
                        InfoDTO dto = new InfoDTO();
                        dto.Id = registro.id_persona;
                        dto.Nombre = registro.nombre;
                        dto.Apellido = registro.apellido;
                        dto.Edad = registro.edad.ToString();
                        dto.Estado = registro.estado.ToString();

                        listado.Add(dto);
                    }

                    r.Listado = listado;
                }
            }
            catch (Exception ex)
            {
                r.Error = ex.InnerException.Message;
            }

            return r;
        }


        public RespuestaDTO AddPersona(string nombre, string apellido, string edad)
        {
            RespuestaDTO dto = new RespuestaDTO();

            int idEdad = 0;
            int.TryParse(edad, out idEdad);


            if (idEdad == 0)
            {
                dto.Error = "El valor ingresado en id no es un dato numérico";
                return dto;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    persona p = new persona();
                    p.nombre = nombre.Trim().ToUpper();
                    p.apellido = apellido.Trim().ToUpper();
                    p.edad = idEdad;
                    p.fecha = DateTime.Now;
                    p.estado = 1;

                    db.Entry(p).State = EntityState.Added;
                    db.SaveChanges();

                    transaction.Commit();

                    dto.Respuesta = "Persona almacenada exitosamente";
                }
                catch (Exception ex)
                {
                    dto.Error = ex.InnerException.Message;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return dto;
        }


        public RespuestaDTO UpdatePersona(string id, string nombre, string apellido, string edad)
        {
            RespuestaDTO dto = new RespuestaDTO();

            int idPersona = 0;
            int idEdad = 0;
            int.TryParse(id, out idPersona);
            int.TryParse(edad, out idEdad);

            if (idPersona == 0)
            {
                dto.Error = "El valor ingresado en id no es un dato numérico";
                return dto;
            }

            if (idEdad == 0)
            {
                dto.Error = "El valor ingresado en edad no es un dato numérico";
                return dto;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var personaDB = db.persona.Where(item => item.id_persona == idPersona).FirstOrDefault();

                    if (personaDB == null)
                        dto.Error = "No existe la persona con id " + idPersona + " para ser actualizada";

                    else
                    {
                        personaDB.nombre = nombre.Trim().ToUpper();
                        personaDB.apellido = apellido.Trim().ToUpper();
                        personaDB.edad = idEdad;
                        personaDB.fecha = DateTime.Now;
                        personaDB.estado = 1;

                        db.Entry(personaDB).State = EntityState.Modified;
                        db.SaveChanges();

                        transaction.Commit();

                        dto.Respuesta = "Persona modificada exitosamente";
                    }
                }
                catch (Exception ex)
                {
                    dto.Error = ex.InnerException.Message;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return dto;
        }


        public RespuestaDTO DeletePersona(string id)
        {
            RespuestaDTO dto = new RespuestaDTO();

            int idPersona = 0;
            int.TryParse(id, out idPersona);

            if (idPersona == 0)
            {
                dto.Error = "El valor ingresado en id no es un dato numérico";
                return dto;
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var personaDB = db.persona.Where(item => item.id_persona == idPersona).FirstOrDefault();

                    if (personaDB == null)
                        dto.Error = "No existe la persona con id " + idPersona + " para ser eliminada";

                    else
                    {

                        db.Entry(personaDB).State = EntityState.Deleted;
                        db.SaveChanges();

                        transaction.Commit();

                        dto.Respuesta = "Persona eliminada exitosamente";
                    }
                }
                catch (Exception ex)
                {
                    dto.Error = ex.InnerException.Message;
                }
                finally
                {
                    transaction.Dispose();
                }
            }

            return dto;
        }


    }
}
