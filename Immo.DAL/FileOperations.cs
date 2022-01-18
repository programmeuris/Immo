using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Immo.Models;

namespace Immo.DAL
{
    public class FileOperations
    {
        private readonly char _valueSeparator = ';';
        private readonly string _cultureString = "nl-BE";

        // if file exists, return all lines as array
        // if not, return null
        private string[] ReadToArr(string fName) => File.Exists(fName) ?
            File.ReadAllLines(fName) : null;

        public bool EigendomIsUniq(List<Eigendom> eigendommen, Eigendom eigendom)
        {
            if (eigendom == null)
            {
                return false;
            }

            bool isUniq = true;

            // if new eigendom equals eigendom in list, return false

            for (int i = 0; i < eigendommen.Count && isUniq; i++)
            {
                // does not work with ! in front instead of == false
                // now it suddenly does...
                isUniq = !eigendom.Equals(eigendommen[i]);
            }

            return isUniq;
        }

        public List<Eigendom> BestandInlezen(string bestand)
        {
            // appartement waterstraat had 600 als bewoonbare oppervlakte en 60 als totaal oppervlakte
            // dit heb ik aangepast omdat beiden numerieke waarden zijn dus lastig om te vangen
            // eventueel had ik kunnen controleren of bewoonbare oppervlakte kleiner of gelijk is aan totaaloppervlakte
            // maar dan weet je nog niet wat de effectieve woonoppervlakte is
            // omdat het appartement waterstraat in de opgave ook een bewoonbare oppervlakte heeft van
            // 60 ging ik er van uit dat dit mocht aangepast worden in het bestand

            var lines = ReadToArr(bestand);

            if (lines == null)
            {
                throw new FileNotFoundException($"Bestand {bestand} bestaat niet!");
            }

            var eigendommen = new List<Eigendom>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    // skip empty lines
                    continue;
                }

                var split = line.Split(_valueSeparator);

                Eigendom eigendom;

                int prijs, bouwjaar, bewoonbareOppervlakte;
                double oppervlakte;

                try
                {
                    switch (split[0])
                    {
                        case "eigendom":
                            if (int.TryParse(
                                split[3].Trim(),
                                NumberStyles.Currency,
                                new CultureInfo(_cultureString),
                                out prijs) &&
                                double.TryParse(
                                split[2].Trim(),
                                NumberStyles.Float,
                                new CultureInfo(_cultureString),
                                out oppervlakte))
                            {
                                eigendom = new Eigendom(prijs,
                                    oppervlakte,
                                    split[1].Trim());

                                if (EigendomIsUniq(eigendommen, eigendom))
                                {
                                    eigendommen.Add(eigendom);
                                }
                                else
                                {
                                    throw new CustomException($"{eigendom.GetType().Name} is een duplicaat");
                                }

                                // reset values
                                //prijs = -1;
                                //oppervlakte = -1;
                            }
                            else
                            {
                                throw new CustomException("Datalijn is corrupt");
                            }
                            break;
                        case "huis":
                            if (int.TryParse(
                                split[3].Trim(),
                                NumberStyles.Currency,
                                new CultureInfo(_cultureString),
                                out prijs) &&
                                double.TryParse(
                                split[2].Trim(),
                                NumberStyles.Float,
                                new CultureInfo(_cultureString),
                                out oppervlakte) &&
                                int.TryParse(
                                split[4].Trim(),
                                NumberStyles.Integer,
                                new CultureInfo(_cultureString),
                                out bouwjaar) &&
                                int.TryParse(
                                split[5].Trim(),
                                NumberStyles.Integer,
                                new CultureInfo(_cultureString),
                                out bewoonbareOppervlakte))
                            {
                                eigendom = new Huis(prijs,
                                    oppervlakte,
                                    split[1].Trim(),
                                    bouwjaar,
                                    bewoonbareOppervlakte);

                                if (EigendomIsUniq(eigendommen, eigendom))
                                {
                                    eigendommen.Add(eigendom);
                                }
                                else
                                {
                                    throw new CustomException($"{eigendom.GetType().Name} is een duplicaat");
                                }

                                // reset values
                                //prijs = -1;
                                //oppervlakte = -1;
                                //bouwjaar = -1;
                                //bewoonbareOppervlakte = -1;
                            }
                            else
                            {
                                throw new CustomException("Datalijn is corrupt");
                            }
                            break;
                        case "appartement":
                            if (int.TryParse(
                                split[3].Trim(),
                                NumberStyles.Currency,
                                new CultureInfo(_cultureString),
                                out prijs) &&
                                double.TryParse(
                                split[2].Trim(),
                                NumberStyles.Float,
                                new CultureInfo(_cultureString),
                                out oppervlakte) &&
                                int.TryParse(
                                split[4].Trim(),
                                NumberStyles.Integer,
                                new CultureInfo(_cultureString),
                                out bouwjaar) &&
                                int.TryParse(
                                split[5].Trim(),
                                NumberStyles.Integer,
                                new CultureInfo(_cultureString),
                                out bewoonbareOppervlakte))
                            {
                                eigendom = new Appartement(
                                    prijs,
                                    oppervlakte,
                                    split[1].Trim(),
                                    bouwjaar,
                                    bewoonbareOppervlakte);

                                if (EigendomIsUniq(eigendommen, eigendom))
                                {
                                    eigendommen.Add(eigendom);
                                }
                                else
                                {
                                    throw new CustomException($"{eigendom.GetType().Name} is een duplicaat");
                                }

                                // reset values
                                //prijs = -1;
                                //oppervlakte = -1;
                                //bouwjaar = -1;
                                //bewoonbareOppervlakte = -1;
                            }
                            else
                            {
                                throw new CustomException("Datalijn is corrupt");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    FoutLoggen(ex);
                }
            }

            return eigendommen.Count > 0 ?
                eigendommen : null;
        }

        public void FoutLoggen(Exception exception, string logFileName = "immo.log.txt")
        {
            string msg =
                $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\n" +
                $"{exception.GetType().Name}\n" +
                $"{exception.Message}\n" +
                $"{exception.StackTrace}\n" +
                new string('-', 69) + "\n";

            File.AppendAllText(logFileName, msg);
        }
    }
}
