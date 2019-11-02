﻿using Microsoft.VisualBasic.FileIO;
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
        public int BrickEditionNumber { get; set; }

        //private static double TextHeight = 0.222;
        private static double TextHeight = 0.16;

        public BrickInscription(string artistName, string country, string lyricLine1, string lyricLine2, string lyricLine3, string lyricLine4, string donorName, int brickEditionNumber)
        {
            ArtistName = artistName;
            Country = country;
            DonorName = donorName;
            LyricLine1 = lyricLine1;
            LyricLine2 = lyricLine2;
            LyricLine3 = lyricLine3;
            LyricLine4 = lyricLine4;
            BrickEditionNumber = brickEditionNumber;
        }

        public override string ToString()
        {
            return "ArtistName: " + this.ArtistName + ", Country: " + this.Country + ", DonorName: " + this.DonorName;
        }

        public static List<BrickInscription> ParseBrickInscriptionsFromCSV(string csvFilePath, int brickEditionNumber)
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
                            fields[indexOfArtistNameColumn + 6],
                            brickEditionNumber
                        );

                    inscriptions.Add(inscription);

                    brickEditionNumber++;
                }
            }

            return inscriptions;
        }

        public static void PrintInscriptionsToDXFFile(List<BrickInscription> inscriptions)
        {
            foreach (BrickInscription inscription in inscriptions)
            {
                List<EntityObject> entities = new List<EntityObject>();

                MText donorName = BrickInscription.CreateMTextAtPosition(inscription.DonorName, 0.33, .70);
                MText lylric1 = BrickInscription.CreateMTextAtPosition(inscription.LyricLine1, 0.35, 3.85);
                MText lylric2 = BrickInscription.CreateMTextAtPosition(inscription.LyricLine2, 0.35, 3.34);
                MText lylric3 = BrickInscription.CreateMTextAtPosition(inscription.LyricLine3, 0.35, 2.82);
                MText lylric4 = BrickInscription.CreateMTextAtPosition(inscription.LyricLine4, 0.35, 2.30);
                MText artistName = BrickInscription.CreateMTextAtPosition(inscription.ArtistName, 3.8, 1.6);
                MText country = BrickInscription.CreateMTextAtPosition(inscription.Country, 3.7, 0.70);
                MText brickEditionNumber = BrickInscription.CreateMTextAtPosition("#" + inscription.BrickEditionNumber.ToString(), 5.7, 4.15, 0.14);

                entities.Add(donorName);
                entities.Add(lylric1);
                entities.Add(lylric2);
                entities.Add(lylric3);
                entities.Add(lylric4);
                entities.Add(artistName);
                entities.Add(country);
                entities.Add(brickEditionNumber);

                string docName = BrickInscription.GenerateFileNameFromInscription(inscription);
                BrickInscription.CreateDXFDocumentWithEntities(docName, entities);
            }
        }

        public static MText CreateMTextAtPosition(string textContent, double x, double y, double textHeight = 0.16)
        {
            TextStyle style = new TextStyle("Another Typewriter.ttf");

            MText text = new MText(new Vector2(x, y), textHeight, 0, style);
            text.Value = textContent;

            return text;
        }

        public static void CreateDXFDocumentWithEntities(string docName, List<EntityObject> entities)
        {
            DxfDocument dxf = new DxfDocument(DxfVersion.AutoCad2010);
            
            foreach (EntityObject entity in entities)
            {
                dxf.AddEntity(entity);
            }

            dxf.Save(docName);
        }

        private static int GetIndexOfArtistNameInHeaderRow(string[] headerRow)
        {
            return Array.IndexOf<string>(headerRow, "Artist name (required)");
        }

        private static string GenerateFileNameFromInscription(BrickInscription inscription)
        {
            return inscription.DonorName + "_" + inscription.ArtistName + "_" + DateTime.Now.ToString("MM-dd-yyyy-h-mmtt") + new Random().Next().ToString() + ".dxf";
        }
    }

}
