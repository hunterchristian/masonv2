using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

using netDxf;
using netDxf.Entities;
using netDxf.Header;
using netDxf.Tables;

namespace Mason
{
    class BrickInscription
    {
        public string ArtistName { get; set; }
        public string Country { get; set; }
        public string DonorName { get; set; }
        public string LyricLine1 { get; set; }
        public string LyricLine2 { get; set; }
        public string LyricLine3 { get; set; }
        public string LyricLine4 { get; set; }

        public BrickInscription(string artistName, string country, string lyricLine1, string lyricLine2, string lyricLine3, string lyricLine4, string donorName)
        {
            ArtistName = artistName;
            Country = country;
            DonorName = donorName;
            LyricLine1 = lyricLine1;
            LyricLine2 = lyricLine2;
            LyricLine3 = lyricLine3;
            LyricLine4 = lyricLine4;
        }

        public static List<BrickInscription> ParseBrickInscriptionsFromCSV(string csvFilePath)
        {
            List<BrickInscription> inscriptions = new List<BrickInscription>();

            using (TextFieldParser parser = new TextFieldParser(csvFilePath))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                string[] headerRow = parser.ReadFields();
                int indexOfArtistNameColumn = BrickInscription.GetIndexOfArtistNameInHeaderRow(headerRow);

                while (!parser.EndOfData)
                {
                    //Process row
                    string[] fields = parser.ReadFields();

                    BrickInscription inscription = new BrickInscription(
                            fields[indexOfArtistNameColumn],
                            fields[indexOfArtistNameColumn + 1],
                            fields[indexOfArtistNameColumn + 2],
                            fields[indexOfArtistNameColumn + 3],
                            fields[indexOfArtistNameColumn + 4],
                            fields[indexOfArtistNameColumn + 5],
                            fields[indexOfArtistNameColumn + 6]
                        );

                    inscriptions.Add(inscription);
                }
            }

            return inscriptions;
        }

        public static void PrintInscriptionsToDXFFile(List<BrickInscription> inscriptions)
        {
            foreach (BrickInscription inscription in inscriptions)
            {
                TextStyle style = new TextStyle("Another Typewriter.ttf");

                MText text1 = new MText(Vector2.Zero, 10, 0, style);
                // you can set manually the text value with all available formatting commands
                text1.Value = inscription.DonorName;

                // both text1 and text2 should yield to the same result
                DxfDocument dxf = new DxfDocument(DxfVersion.AutoCad2010);
                dxf.AddEntity(text1);

                dxf.Save(BrickInscription.GenerateFileNameFromInscription(inscription));
            }
        }

        private static int GetIndexOfArtistNameInHeaderRow(string[] headerRow)
        {
            return Array.IndexOf<string>(headerRow, "Artist name (required)");
        }

        private static string GenerateFileNameFromInscription(BrickInscription inscription)
        {
            return inscription.DonorName + "_" + inscription.ArtistName + "_" + DateTime.Now.ToString("MM-dd-yyyy-h-mmtt") + ".dxf";
        }
    }

}
