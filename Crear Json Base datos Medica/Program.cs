using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Crear_Json_Base_datos_Medica
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string connectionString = "Data Source=LAPTOP-UB7952GK\\MSSQLSERVER01;Initial Catalog=db_olivera;Integrated Security=True";
            string query = "SELECT Top(100)[Expediente] FROM [db_olivera].[dbo].[pacientes] where Expediente IS NOT NULL AND Expediente != ''"; // Ajusta esta línea según tu consulta SQL.

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        rows.Add(row);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            try
            {

                foreach (var rowindex in rows)
                {
                    foreach (var rowObject in rowindex)
                    {


                        Expediente expediente = CrearYRellenarexpediente(rowObject.Value.ToString());
                        string json = System.Text.Json.JsonSerializer.Serialize(expediente, new JsonSerializerOptions { WriteIndented = true });
                        Console.WriteLine(json);
                        Console.WriteLine("Introduce el texto para el documento Word:");
                        //string texto = Console.ReadLine();

                        //string filePath = "DocumentoWord.docx";
                        //CrearDocumentoWord(filePath, rowObject.Value.ToString());

                        Console.WriteLine("Documento Word creado con éxito.");
                        //    // Dividir el texto en líneas
                        //    string[] lineas = rowObject.Value.ToString().Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

                        //    // Objeto para almacenar la estructura jerárquica
                        //    var estructura = new Dictionary<string, object>();

                        //    // Variables temporales para almacenar las claves de nivel superior
                        //    string claveActual = null;

                        //    foreach (var linea in lineas)
                        //    {
                        //        // Identificar si la línea es una clave de nivel superior
                        //        if (linea.ToUpper().Contains("EXPEDIENTE:") || linea.ToUpper().Contains("HISTORIA CLINICA DE PATOLOGIA MAMARIA") || linea.ToUpper().Contains("HISTORIA DE LA DESCENDENCIA"))
                        //        {
                        //            claveActual = linea.Trim();
                        //            estructura[claveActual] = new Dictionary<string, string>();
                        //        }
                        //        else if (claveActual != null)
                        //        {
                        //            // Separar la línea en clave y valor
                        //            var partes = linea.Split(new[] { ':' }, 2);
                        //            if (partes.Length == 2)
                        //            {
                        //                var clave = partes[0].Trim();
                        //                var valor = partes[1].Trim();
                        //                ((Dictionary<string, string>)estructura[claveActual]).Add(clave, valor);
                        //            }
                        //        }
                        //    }

                        //    // Convertir la estructura a JSON
                        //    string json = JsonConvert.SerializeObject(estructura, Formatting.Indented);
                        //}

                    }

                }
            }
            catch (Exception ex)
            {

            }
          

        }

        private static Expediente CrearYRellenarexpediente(string texto)
        {
            var expediente = new Expediente
            {
                InformacionPersonal = new InformacionPersonal(),
                AntecedentesFamiliares = new AntecedentesFamiliares(),
                AntecedentesPersonales = new AntecedentesPersonales
                {
                    NoPatologicos = new AntecedentesPersonalesNoPatologicos(),
                    Patologicos = new AntecedentesPersonalesPatologicos()
                },
                AntecedentesGinecobstetricos = new AntecedentesGinecobstetricos(),
                AntecedentesQuirurgicos = new AntecedentesQuirurgicos(),
                PadecimientoActual = new PadecimientoActual(),
                ExamenFisicoGeneral = new ExamenFisicoGeneral(),
                ObservacionesTratamiento = new ObservacionesTratamiento()
            };
            ////////////////////////Informacion Personal//////////////////////////
            // Fecha de Nacimiento
            var fechaNacRegex = new Regex(@"FECHA DE NACIMIENTO\s+(\d{1,2}/\d{1,2}/\d{2,4})");
            var matchFechaNac = fechaNacRegex.Match(texto);
            if (matchFechaNac.Success)
            {
                expediente.InformacionPersonal.FechaDeNacimiento = DateTime.ParseExact(matchFechaNac.Groups[1].Value, "d/M/yy", CultureInfo.InvariantCulture);
            }

            // Sexo
            var sexoRegex = new Regex(@"SEXO\s+([FM])");
            var matchSexo = sexoRegex.Match(texto);
            if (matchSexo.Success)
            {
                expediente.InformacionPersonal.Sexo = matchSexo.Groups[1].Value[0];
            }

            // Estado Civil
            expediente.InformacionPersonal.EstadoCivil = Regex.Match(texto, @"ESTADO CIVIL\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Ocupacion
            expediente.InformacionPersonal.Ocupacion = Regex.Match(texto, @"OCUPACION\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Domicilio
            expediente.InformacionPersonal.Domicilio = Regex.Match(texto, @"DOMICILIO\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Poblacion
            expediente.InformacionPersonal.Poblacion = Regex.Match(texto, @"POBLACION\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Telefono
            expediente.InformacionPersonal.Telefono = Regex.Match(texto, @"TELEFONO\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Email
            expediente.InformacionPersonal.Email = Regex.Match(texto, @"E-MAIL\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Nombre del Esposo
            expediente.InformacionPersonal.NombreDelEsposo = Regex.Match(texto, @"NOMBRE DEL ESPOSO\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Edad del Esposo
            var edadEsposoRegex = Regex.Match(texto, @"EDAD DEL ESPOSO\s+(\d+)");
            if (edadEsposoRegex.Success)
            {
                expediente.InformacionPersonal.EdadDelEsposo = int.Parse(edadEsposoRegex.Groups[1].Value);
            }

            // Ocupacion Esposo
            expediente.InformacionPersonal.OcupacionEsposo = Regex.Match(texto, @"OCUPACION ESPOSO\s+([^\r\n]+)").Groups[1].Value.Trim();

            // Referencia
            expediente.InformacionPersonal.Referencia = Regex.Match(texto, @"REFERENCIA\s+([^\r\n]+)").Groups[1].Value.Trim();


            /////////////////////////////expediente.AntecedentesFamiliares////////////////////////////////////////
            // Usar expresiones regulares para extraer la información
            expediente.AntecedentesFamiliares.Diabetes = ExtraerValorPorEtiqueta(texto, @"DIABETES\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.Hipertension = ExtraerValorPorEtiqueta(texto, @"HIPERTENSION\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.Trombosis = ExtraerValorPorEtiqueta(texto, @"TROMBOSIS\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.Cardiopatias = ExtraerValorPorEtiqueta(texto, @"CARDIOPATIAS\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.Cancer = ExtraerValorPorEtiqueta(texto, @"CANCER\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.EnfermedadesGeneticas = ExtraerValorPorEtiqueta(texto, @"ENFERMEDADES GENETICAS\s+([^\r\n]+)");
            expediente.AntecedentesFamiliares.OtraEnfermedad = ExtraerValorPorEtiqueta(texto, @"OTRA ENFERMEDAD\s+([^\r\n]+)");

            ///////////////////////////expediente.Antecedentes personales no patologicos///////////////////////////////////////
            expediente.AntecedentesPersonales.NoPatologicos.Inmunizaciones = ExtraerValorPorEtiqueta(texto, "INMUNIZACIONES");
            expediente.AntecedentesPersonales.NoPatologicos.Alcoholismo = ExtraerValorPorEtiqueta(texto, "ALCOHOLISMO");
            expediente.AntecedentesPersonales.NoPatologicos.Tabaquismo = ExtraerValorPorEtiqueta(texto, "TABAQUISMO");
            expediente.AntecedentesPersonales.NoPatologicos.TabaquismoPasivo = ExtraerValorPorEtiqueta(texto, "TABAQUISMO PASIVO");
            expediente.AntecedentesPersonales.NoPatologicos.DrogasOMedicamentos = ExtraerValorPorEtiqueta(texto, "DROGAS O MEDICAMENTOS");
            expediente.AntecedentesPersonales.NoPatologicos.GrupoSanguineo = ExtraerValorPorEtiqueta(texto, "GRUPO SANGUÍNEO");

            ///////////////////////////expediente.AntecedentesPersonalesPatologicos///////////////////////////////////////
            expediente.AntecedentesPersonales.Patologicos.PropiasDeLaInfancia = ExtraerValorPorEtiqueta(texto, "PROPIAS DE LA INF.");
            expediente.AntecedentesPersonales.Patologicos.Rubeola = ExtraerValorPorEtiqueta(texto, "RUBEOLA");
            expediente.AntecedentesPersonales.Patologicos.Amigdalitis = ExtraerValorPorEtiqueta(texto, "AMIGDALITIS");
            expediente.AntecedentesPersonales.Patologicos.Bronquitis = ExtraerValorPorEtiqueta(texto, "BRONQUITIS");
            expediente.AntecedentesPersonales.Patologicos.Bronconeumonia = ExtraerValorPorEtiqueta(texto, "BRONCONEUMONIA");
            expediente.AntecedentesPersonales.Patologicos.HepatitisViralTipo = ExtraerValorPorEtiqueta(texto, "HEPATITIS VIRAL TIPO");
            expediente.AntecedentesPersonales.Patologicos.Parasitosis = ExtraerValorPorEtiqueta(texto, "PARASITOSIS");
            expediente.AntecedentesPersonales.Patologicos.Toxoplasmosis = ExtraerValorPorEtiqueta(texto, "TOXOPLASMOSIS");
            expediente.AntecedentesPersonales.Patologicos.Citomegalovirus = ExtraerValorPorEtiqueta(texto, "CITOMEGALOVIRUS");
            expediente.AntecedentesPersonales.Patologicos.Herpes = ExtraerValorPorEtiqueta(texto, "HERPES");
            expediente.AntecedentesPersonales.Patologicos.Clamydiasis = ExtraerValorPorEtiqueta(texto, "CLAMYDIASIS");
            expediente.AntecedentesPersonales.Patologicos.HIV = ExtraerValorPorEtiqueta(texto, "HIV");
            expediente.AntecedentesPersonales.Patologicos.Sifilis = ExtraerValorPorEtiqueta(texto, "SIFILIS");
            expediente.AntecedentesPersonales.Patologicos.Micosis = ExtraerValorPorEtiqueta(texto, "MICOSIS");
            expediente.AntecedentesPersonales.Patologicos.EIP = ExtraerValorPorEtiqueta(texto, "EIP");
            expediente.AntecedentesPersonales.Patologicos.Hipertension = ExtraerValorPorEtiqueta(texto, "HIPERTENSION");
            expediente.AntecedentesPersonales.Patologicos.DiabetesMellitus = ExtraerValorPorEtiqueta(texto, "DIABETES MELLITUS");
            expediente.AntecedentesPersonales.Patologicos.OtrasEndocrinas = ExtraerValorPorEtiqueta(texto, "OTRAS ENDOCRINAS");
            expediente.AntecedentesPersonales.Patologicos.Cardiopatias = ExtraerValorPorEtiqueta(texto, "CARDIOPATIAS");
            expediente.AntecedentesPersonales.Patologicos.Nefropatias = ExtraerValorPorEtiqueta(texto, "NEFROPATIAS");
            expediente.AntecedentesPersonales.Patologicos.Digestivas = ExtraerValorPorEtiqueta(texto, "DIGESTIVAS");
            expediente.AntecedentesPersonales.Patologicos.Neurologicas = ExtraerValorPorEtiqueta(texto, "NEUROLOGICAS");
            expediente.AntecedentesPersonales.Patologicos.Hematologicas = ExtraerValorPorEtiqueta(texto, "HEMATOLOGICAS");
            expediente.AntecedentesPersonales.Patologicos.Tumores = ExtraerValorPorEtiqueta(texto, "TUMORES");
            expediente.AntecedentesPersonales.Patologicos.Condilomatosis = ExtraerValorPorEtiqueta(texto, "CONDILOMATOSIS");
            expediente.AntecedentesPersonales.Patologicos.Displasias = ExtraerValorPorEtiqueta(texto, "DISPLASIAS");
            expediente.AntecedentesPersonales.Patologicos.Alergia = ExtraerValorPorEtiqueta(texto, "ALERGIA");

            ///////////////////////////expediente.AntecedentesGinecobstetricos///////////////////////////////////////
            expediente.AntecedentesGinecobstetricos.Menarca = ExtraerValorPorEtiqueta(texto, "MENARCA");
            expediente.AntecedentesGinecobstetricos.Ritmo = ExtraerValorPorEtiqueta(texto, "RITMO");
            expediente.AntecedentesGinecobstetricos.IVSA = ExtraerValorPorEtiqueta(texto, "IVSA");
            expediente.AntecedentesGinecobstetricos.Gesta = ExtraerValorPorEtiqueta(texto, "GESTA");
            expediente.AntecedentesGinecobstetricos.Para = ExtraerValorPorEtiqueta(texto, "PARA");
            expediente.AntecedentesGinecobstetricos.FUP = DateTime.TryParse(ExtraerValorPorEtiqueta(texto, "FUP"), out DateTime fup) ? fup : (DateTime?)null;
            expediente.AntecedentesGinecobstetricos.Abortos = ExtraerValorPorEtiqueta(texto, "ABORTOS");
            expediente.AntecedentesGinecobstetricos.FUA = DateTime.TryParse(ExtraerValorPorEtiqueta(texto, "FUA"), out DateTime fua) ? fua : (DateTime?)null;
            expediente.AntecedentesGinecobstetricos.Cesareas = ExtraerValorPorEtiqueta(texto, "CESAREAS");
            expediente.AntecedentesGinecobstetricos.FUC = DateTime.TryParse(ExtraerValorPorEtiqueta(texto, "FUC"), out DateTime fuc) ? fuc : (DateTime?)null;
            expediente.AntecedentesGinecobstetricos.EmbarazoEctopico = ExtraerValorPorEtiqueta(texto, "EMBARAZO ECTOPICO");
            expediente.AntecedentesGinecobstetricos.FechaEmbEctopico = DateTime.TryParse(ExtraerValorPorEtiqueta(texto, "FECHA EMB ECTOPICO"), out DateTime fechaEmbEctopico) ? fechaEmbEctopico : (DateTime?)null;
            expediente.AntecedentesGinecobstetricos.Endometriosis = ExtraerValorPorEtiqueta(texto, "ENDOMETRIOSIS");
            expediente.AntecedentesGinecobstetricos.FUM = DateTime.TryParse(ExtraerValorPorEtiqueta(texto, "FUM"), out DateTime fum) ? fum : (DateTime?)null;
            expediente.AntecedentesGinecobstetricos.InfertilidadPrimaria = ExtraerValorPorEtiqueta(texto, "INFERTILIDAD PRIMARIA");
            expediente.AntecedentesGinecobstetricos.InfertilidadSecundaria = ExtraerValorPorEtiqueta(texto, "INFERTILIDAD SECUNDAR");
            expediente.AntecedentesGinecobstetricos.Anticoncepcion = ExtraerValorPorEtiqueta(texto, "ANTICONCEPCION");
            expediente.AntecedentesGinecobstetricos.Dismenorrea = ExtraerValorPorEtiqueta(texto, "DISMENORREA");
            expediente.AntecedentesGinecobstetricos.Galactorrea = ExtraerValorPorEtiqueta(texto, "GALACTORREA");
            expediente.AntecedentesGinecobstetricos.Leucorrea = ExtraerValorPorEtiqueta(texto, "LEUCORREA");
            expediente.AntecedentesGinecobstetricos.Dispareunia = ExtraerValorPorEtiqueta(texto, "DISPAREUNIA");
            expediente.AntecedentesGinecobstetricos.IncontinenciaUrinaria = ExtraerValorPorEtiqueta(texto, "INCONTINENCIA URINARIA");
            expediente.AntecedentesGinecobstetricos.IncontinenciaAlToser = ExtraerValorPorEtiqueta(texto, "AL TOSER");
            expediente.AntecedentesGinecobstetricos.IncontinenciaAlReirse = ExtraerValorPorEtiqueta(texto, "AL REIRSE");
            expediente.AntecedentesGinecobstetricos.IncontinenciaAlBrincar = ExtraerValorPorEtiqueta(texto, "AL BRINCAR");
            expediente.AntecedentesGinecobstetricos.IncontinenciaAlEstornudar = ExtraerValorPorEtiqueta(texto, "AL ESTORNUDAR");
            expediente.AntecedentesGinecobstetricos.IncontinenciaAlAgacharse = ExtraerValorPorEtiqueta(texto, "AL AGACHARSE");
            expediente.AntecedentesGinecobstetricos.Citologia = ExtraerValorPorEtiqueta(texto, "CITOLOGIA");
            expediente.AntecedentesGinecobstetricos.Mamografia = ExtraerValorPorEtiqueta(texto, "MAMOGRAFIA");
            expediente.AntecedentesGinecobstetricos.Cauterizaciones = ExtraerValorPorEtiqueta(texto, "CAUTERIZACIONES");
            expediente.AntecedentesGinecobstetricos.CirugiasGinecologicas = ExtraerValorPorEtiqueta(texto, "CIRUGIAS GINECOLOGICAS");
            expediente.AntecedentesGinecobstetricos.Laboratorio = ExtraerValorPorEtiqueta(texto, "LABORATORIO");
            expediente.AntecedentesGinecobstetricos.Gabinete = ExtraerValorPorEtiqueta(texto, "GABINETE");
            expediente.AntecedentesGinecobstetricos.TxPrevio = ExtraerValorPorEtiqueta(texto, "TX PREVIOS");


            ///////////////////////////expediente.AntecedentesQuirurgicos///////////////////////////////////////
            expediente.AntecedentesQuirurgicos.Amigdalas = ExtraerValorPorEtiqueta(texto, "AMIGDALAS");
            expediente.AntecedentesQuirurgicos.Hernias = ExtraerValorPorEtiqueta(texto, "HERNIAS");
            expediente.AntecedentesQuirurgicos.Apendice = ExtraerValorPorEtiqueta(texto, "APENDICE");
            expediente.AntecedentesQuirurgicos.Vesicula = ExtraerValorPorEtiqueta(texto, "VESICULA");
            expediente.AntecedentesQuirurgicos.Miopia = ExtraerValorPorEtiqueta(texto, "MIOPIA");
            expediente.AntecedentesQuirurgicos.Histerectomia = ExtraerValorPorEtiqueta(texto, "HISTERECTOMIA");
            expediente.AntecedentesQuirurgicos.Miomectomia = ExtraerValorPorEtiqueta(texto, "MIOMECTOMIA");
            expediente.AntecedentesQuirurgicos.OTB = ExtraerValorPorEtiqueta(texto, "OTB");
            expediente.AntecedentesQuirurgicos.Cerclajes = ExtraerValorPorEtiqueta(texto, "CERCLAJES");
            expediente.AntecedentesQuirurgicos.Laparotomia = ExtraerValorPorEtiqueta(texto, "LAPAROTOMIA");
            expediente.AntecedentesQuirurgicos.Laparoscopia = ExtraerValorPorEtiqueta(texto, "LAPAROSCOPIA");
            expediente.AntecedentesQuirurgicos.Histeroscopia = ExtraerValorPorEtiqueta(texto, "HISTEROSCOPIA");
            expediente.AntecedentesQuirurgicos.RecanalizacionTubaria = ExtraerValorPorEtiqueta(texto, "RECANALIZACION TUB.");
            expediente.AntecedentesQuirurgicos.Colpoperinorrafia = ExtraerValorPorEtiqueta(texto, "COLPOPERINORRAFIA");
            expediente.AntecedentesQuirurgicos.TraquelectomiaOCono = ExtraerValorPorEtiqueta(texto, "TRAQUELECTOMIA O CONO");
            expediente.AntecedentesQuirurgicos.Criocirugia = ExtraerValorPorEtiqueta(texto, "CRIOCIRUGIA");
            expediente.AntecedentesQuirurgicos.Laseterapia = ExtraerValorPorEtiqueta(texto, "LASERTERAPIA");
            expediente.AntecedentesQuirurgicos.Cauterizacion = ExtraerValorPorEtiqueta(texto, "CAUTERIZACION");
            expediente.AntecedentesQuirurgicos.EmbarazoEctopico = ExtraerValorPorEtiqueta(texto, "EMB. ECTOPICO");
            expediente.AntecedentesQuirurgicos.Anexectomia = ExtraerValorPorEtiqueta(texto, "ANEXECTOMIA");
            expediente.AntecedentesQuirurgicos.FibroadenomaDeMama = ExtraerValorPorEtiqueta(texto, "FIBROADENOMA DE MAMA");
            expediente.AntecedentesQuirurgicos.Mastopexia = ExtraerValorPorEtiqueta(texto, "MASTOPEXIA");
            expediente.AntecedentesQuirurgicos.ImplantesMamarios = ExtraerValorPorEtiqueta(texto, "IMPLANTES MAMARIOS");
            expediente.AntecedentesQuirurgicos.Abdominoplastia = ExtraerValorPorEtiqueta(texto, "ABDOMINOPLASTIA");
            expediente.AntecedentesQuirurgicos.Rinoplastia = ExtraerValorPorEtiqueta(texto, "RINOPLASTIA");
            expediente.AntecedentesQuirurgicos.Blefaroplastia = ExtraerValorPorEtiqueta(texto, "BLEFAROPLASTIA");
            expediente.AntecedentesQuirurgicos.RitidectomiaFacial = ExtraerValorPorEtiqueta(texto, "RITIDECTOMIA FACIAL");
            expediente.AntecedentesQuirurgicos.Lipoescultura = ExtraerValorPorEtiqueta(texto, "LIPOESCULTURA");
            expediente.AntecedentesQuirurgicos.Bichectomia = ExtraerValorPorEtiqueta(texto, "BICHECTOMIA");
            expediente.AntecedentesQuirurgicos.Artroscopia = ExtraerValorPorEtiqueta(texto, "ARTROSCOPIA");
            expediente.AntecedentesQuirurgicos.PlastiaHalluxValgus = ExtraerValorPorEtiqueta(texto, "PLASTIA HALLUX VALGUS");


            ///////////////////////////expediente.PadecimientoActual///////////////////////////////////////
            expediente.PadecimientoActual.Descripcion = ExtraerValorPorEtiqueta(texto, "PADECIMIENTO ACTUAL");

            ///////////////////////////expediente.ExamenFisicoGeneral///////////////////////////////////////
            expediente.ExamenFisicoGeneral.Peso = ExtraerValorPorEtiqueta(texto, "PESO");
            expediente.ExamenFisicoGeneral.Talla = ExtraerValorPorEtiqueta(texto, "TALLA");
            expediente.ExamenFisicoGeneral.Valoracion_Ponderadal = ExtraerValorPorEtiqueta(texto, "VALORACION PONDERAL");
            expediente.ExamenFisicoGeneral.Ta = ExtraerValorPorEtiqueta(texto, "TA");
            expediente.ExamenFisicoGeneral.Temp = ExtraerValorPorEtiqueta(texto, "TEMP");
            expediente.ExamenFisicoGeneral.Pulso = ExtraerValorPorEtiqueta(texto, "PULSO");
            expediente.ExamenFisicoGeneral.Cabeza = ExtraerValorPorEtiqueta(texto, "CABEZA");
            expediente.ExamenFisicoGeneral.Cara = ExtraerValorPorEtiqueta(texto, "CARA");
            expediente.ExamenFisicoGeneral.Mucosas = ExtraerValorPorEtiqueta(texto, "MUCOSA");
            expediente.ExamenFisicoGeneral.Cuello = ExtraerValorPorEtiqueta(texto, "CUELLO");
            expediente.ExamenFisicoGeneral.Torax = ExtraerValorPorEtiqueta(texto, "TORAX");
            expediente.ExamenFisicoGeneral.Columna = ExtraerValorPorEtiqueta(texto, "COLUMNA");
            expediente.ExamenFisicoGeneral.Mamas = ExtraerValorPorEtiqueta(texto, "MAMAS");
            expediente.ExamenFisicoGeneral.Axilas = ExtraerValorPorEtiqueta(texto, "AXILAS");
            expediente.ExamenFisicoGeneral.Fosa_Supraclavicular = ExtraerValorPorEtiqueta(texto, "FOSA SUPRACLAVICULAR");
            expediente.ExamenFisicoGeneral.Abdomen = ExtraerValorPorEtiqueta(texto, "ABDOMEN");
            expediente.ExamenFisicoGeneral.Extremidades = ExtraerValorPorEtiqueta(texto, "EXTREMIDADES");
            expediente.ExamenFisicoGeneral.Genitales_Externos = ExtraerValorPorEtiqueta(texto, "GENITALES EXTERNOS");
            expediente.ExamenFisicoGeneral.ESPECULOSCOPIA = ExtraerValorPorEtiqueta(texto, "ESPECULOSCOPIA");
            expediente.ExamenFisicoGeneral.TACTO_CERVICOVAGINAL = ExtraerValorPorEtiqueta(texto, "TACTO CERVICOVAGINAL");
            expediente.ExamenFisicoGeneral.REGION_ANAL = ExtraerValorPorEtiqueta(texto, "REGION ANAL");            
            expediente.ExamenFisicoGeneral.GLICEMIA_CAPILAR = ExtraerValorPorEtiqueta(texto, "GLICEMIA CAPILAR");


            ///////////////////////////expediente.ObservacionesTratamiento///////////////////////////////////////
            // Extraer y asignar ID
            expediente.ObservacionesTratamiento.ID = ExtraerValorPorEtiqueta(texto, "ID");

            // Extraer y asignar IndicacionesYReceta
            expediente.ObservacionesTratamiento.IndicacionesYReceta = ExtraerValorPorEtiqueta(texto, "Indicaciones y Receta");

            // Extraer y asignar Pendiente
            expediente.ObservacionesTratamiento.Pendiente = ExtraerValorPorEtiqueta(texto, "PENDIENTE");


            return expediente;
        }

        //private static string ExtraerValorPorEtiqueta(string texto, string patron)
        //{
        //    var regex = new Regex(patron);
        //    var match = regex.Match(texto);
        //    return match.Success ? match.Groups[1].Value.Trim() : "No especificado";
        //}

        private static string ExtraerValorPorEtiqueta(string texto, string etiqueta)
        {   
            var pattern = $@"(?:^|\n){etiqueta}\s+(.*?)(?=\n[A-Z]|\z)";
            var match = Regex.Match(texto, pattern, RegexOptions.Singleline);
            return match.Success ? match.Groups[1].Value.Trim() : "No especificado";
        }

        public static void CrearDocumentoWord(string filePath, string contenido)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(filePath, DocumentFormat.OpenXml.WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Dividir el contenido en líneas para manejar los saltos de línea
                string[] lineas = contenido.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);

                foreach (string linea in lineas)
                {
                    // Añadir un párrafo por cada línea
                    Paragraph para = body.AppendChild(new Paragraph());
                    Run run = para.AppendChild(new Run());

                    // Añadir texto a la línea. Para preservar espacios múltiples, se puede usar el atributo xml:space="preserve"
                    run.AppendChild(new Text(linea) { Space = SpaceProcessingModeValues.Preserve });
                }
            }
        }
    }
}
