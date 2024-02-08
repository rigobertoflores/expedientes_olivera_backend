using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crear_Json_Base_datos_Medica
{
    public class Expediente
    {
        public InformacionPersonal InformacionPersonal { get; set; }
        public AntecedentesFamiliares AntecedentesFamiliares { get; set; }
        public AntecedentesPersonales AntecedentesPersonales { get; set; }
        public AntecedentesGinecobstetricos AntecedentesGinecobstetricos { get; set; }
        public AntecedentesQuirurgicos AntecedentesQuirurgicos { get; set; }
        public PadecimientoActual PadecimientoActual { get; set; }
        public ExamenFisicoGeneral ExamenFisicoGeneral { get; set; }
        public ObservacionesTratamiento ObservacionesTratamiento { get; set; }
    }

    public class InformacionPersonal
    {
        public DateTime FechaDeNacimiento { get; set; }
        public char Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public string Ocupacion { get; set; }
        public string Domicilio { get; set; }
        public string Poblacion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string NombreDelEsposo { get; set; }
        public int? EdadDelEsposo { get; set; }
        public string OcupacionEsposo { get; set; }
        public string Referencia { get; set; }
    }

    public class AntecedentesFamiliares
    {
        public string Diabetes { get; set; }
        public string Hipertension { get; set; }
        public string Trombosis { get; set; }
        public string Cardiopatias { get; set; }
        public string Cancer { get; set; }
        public string EnfermedadesGeneticas { get; set; }
        public string OtraEnfermedad { get; set; }
    }

    public class AntecedentesPersonales
    {
        public AntecedentesPersonalesNoPatologicos NoPatologicos { get; set; }
        public AntecedentesPersonalesPatologicos Patologicos { get; set; }
    }

    public class AntecedentesPersonalesNoPatologicos
    {
        public string Inmunizaciones { get; set; }
        public string Alcoholismo { get; set; }
        public string Tabaquismo { get; set; }
        public string TabaquismoPasivo { get; set; }
        public string DrogasOMedicamentos { get; set; }
        public string GrupoSanguineo { get; set; }
    }

    public class AntecedentesPersonalesPatologicos
    {
        public string PropiasDeLaInfancia { get; set; }
        public string Rubeola { get; set; }
        public string Amigdalitis { get; set; }
        public string Bronquitis { get; set; }
        public string Bronconeumonia { get; set; }
        public string HepatitisViralTipo { get; set; } // Suponiendo que se quiera especificar el tipo
        public string Parasitosis { get; set; }
        public string Toxoplasmosis { get; set; }
        public string Citomegalovirus { get; set; }
        public string Herpes { get; set; }
        public string Clamydiasis { get; set; }
        public string HIV { get; set; }
        public string Sifilis { get; set; }
        public string Micosis { get; set; }
        public string EIP { get; set; } // Enfermedad inflamatoria pélvica
        public string Hipertension { get; set; }
        public string DiabetesMellitus { get; set; }
        public string OtrasEndocrinas { get; set; } // Podría necesitar una especificación más detallada
        public string Cardiopatias { get; set; }
        public string Nefropatias { get; set; }
        public string Digestivas { get; set; }
        public string Neurologicas { get; set; }
        public string Hematologicas { get; set; }
        public string Tumores { get; set; }
        public string Condilomatosis { get; set; }
        public string Displasias { get; set; }
        public string Alergia { get; set; }
    }

    public class AntecedentesGinecobstetricos
    {
        public string Menarca { get; set; }
        public string Ritmo { get; set; }
        public string IVSA { get; set; } // Inicio de Vida Sexual Activa
        public string Gesta { get; set; }
        public string Para { get; set; }
        public DateTime? FUP { get; set; } // Fecha de Última Menstruación
        public string Abortos { get; set; }
        public DateTime? FUA { get; set; } // Fecha de Último Aborto
        public string Cesareas { get; set; }
        public DateTime? FUC { get; set; } // Fecha de Última Cesárea
        public string EmbarazoEctopico { get; set; }
        public DateTime? FechaEmbEctopico { get; set; }
        public string Endometriosis { get; set; }
        public DateTime? FUM { get; set; } // Fecha de Última Mastografía
        public string InfertilidadPrimaria { get; set; }
        public string InfertilidadSecundaria { get; set; }
        public string Anticoncepcion { get; set; } // Podría requerirse especificar el método
        public string Dismenorrea { get; set; }
        public string Galactorrea { get; set; }
        public string Leucorrea { get; set; }
        public string Dispareunia { get; set; }
        public string IncontinenciaUrinaria { get; set; }
        public string IncontinenciaAlToser { get; set; }
        public string IncontinenciaAlReirse { get; set; }
        public string IncontinenciaAlBrincar { get; set; }
        public string IncontinenciaAlEstornudar { get; set; }
        public string IncontinenciaAlAgacharse { get; set; }
        public string Citologia { get; set; } // Puede necesitar detalles sobre resultados
        public string Mamografia { get; set; } // Puede necesitar detalles sobre resultados
        public string Cauterizaciones { get; set; }
        public string CirugiasGinecologicas { get; set; } // Lista de cirugías o descripción
        public string Laboratorio { get; set; } // Detalles de resultados relevantes
        public string Gabinete { get; set; } // Detalles de estudios realizados
        public string TxPrevio { get; set; } // Tratamientos previos
    }

    public class AntecedentesQuirurgicos
    {
        public string Amigdalas { get; set; }
        public string Hernias { get; set; }
        public string Apendice { get; set; }
        public string Vesicula { get; set; }
        public string Miopia { get; set; } // Considerar si se refiere a cirugía correctiva
        public string Histerectomia { get; set; }
        public string Miomectomia { get; set; }
        public string OTB { get; set; } // Oclusión Tubaria Bilateral
        public string Cerclajes { get; set; }
        public string Laparotomia { get; set; }
        public string Laparoscopia { get; set; }
        public string Histeroscopia { get; set; }
        public string RecanalizacionTubaria { get; set; }
        public string Colpoperinorrafia { get; set; }
        public string TraquelectomiaOCono { get; set; }
        public string Criocirugia { get; set; }
        public string Laseterapia { get; set; }
        public string Cauterizacion { get; set; }
        public string EmbarazoEctopico { get; set; }
        public string Anexectomia { get; set; }
        public string FibroadenomaDeMama { get; set; }
        public string Mastopexia { get; set; }
        public string ImplantesMamarios { get; set; }
        public string Abdominoplastia { get; set; }
        public string Rinoplastia { get; set; }
        public string Blefaroplastia { get; set; }
        public string RitidectomiaFacial { get; set; }
        public string Lipoescultura { get; set; }
        public string Bichectomia { get; set; }
        public string Artroscopia { get; set; }
        public string PlastiaHalluxValgus { get; set; }
    }

    public class PadecimientoActual
    {
        public string Descripcion { get; set; }
    }

    public class ExamenFisicoGeneral
    {
        public string Peso { get; set; }
        public string Talla { get; set; }

        public string Valoracion_Ponderadal { get; set; }
        public string Ta { get; set; }
        public string Temp { get; set; }
        public string Pulso { get; set; }
        public string Cabeza { get; set; }
        public string Cara { get; set; }
        public string Mucosas { get; set; }
        public string Cuello { get; set; }

        public string Torax { get; set; }
        public string Columna { get; set; }
        public string Mamas { get; set; }
        public string Axilas { get; set; }
        public string Fosa_Supraclavicular { get; set; }
        public string Abdomen { get; set; }
        public string Extremidades { get; set; }
        public string Genitales_Externos { get; set; }

        public string ESPECULOSCOPIA { get; set; }
        public string TACTO_CERVICOVAGINAL { get; set; }
        public string REGION_ANAL { get; set; }
        public string GLICEMIA_CAPILAR { get; set; }
    }

    public class ObservacionesTratamiento
    {
        public string ID { get; set; }
        public string IndicacionesYReceta { get; set; }
        public string Pendiente { get; set; }

    }

}
