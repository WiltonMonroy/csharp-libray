using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;

namespace ServicioDatos
{
    [DataContract]
    public class RespuestaDTO
    {
        [DataMember]
        public string Error { get; set; }
        [DataMember]
        public string Respuesta { get; set; }
        [DataMember]
        public List<InfoDTO> Listado { get; set; }
    }

    [DataContract]
    public class InfoDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Nombre { get; set; }
        [DataMember]
        public string Estado { get; set; }
        [DataMember]
        public string Apellido { get; set; }
        [DataMember]
        public string Edad { get; set; }
    }

    [ServiceContract]
    public interface IPersona
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Personas",
           ResponseFormat = WebMessageFormat.Json)
        ]
        RespuestaDTO Personas();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "Personas/{id}",
          ResponseFormat = WebMessageFormat.Json)
        ]
        RespuestaDTO Personas2(string id);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Personas/{nombre}/{apellido}/{edad}",
          ResponseFormat = WebMessageFormat.Json)
        ]
        RespuestaDTO AddPersona(string nombre, string apellido, string edad);


        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Personas/{id}/{nombre}/{apellido}/{edad}",
         ResponseFormat = WebMessageFormat.Json)
        ]
        RespuestaDTO UpdatePersona(string id, string nombre, string apellido, string edad);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Personas/{id}",
         ResponseFormat = WebMessageFormat.Json)
        ]
        RespuestaDTO DeletePersona(string id);

    }

}