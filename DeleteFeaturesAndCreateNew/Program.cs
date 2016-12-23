using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeleteFeaturesAndCreateNew
{
 
    class Program
    {
        static string currentDeletion; //Stores current row of Features to Delete
        static HashSet<string> featuresHashSet = new HashSet<string>();
        const Int32 BufferSize = 128;
        const string featureFileName = "Features.txt";
        const string featureDeleteFileName = "FeaturesToDelete.txt";
        const string resultFileName = "NewFeaturesWithoutDeleted.txt";

        static void Main(string[] args)
        {

         Console.WriteLine("start "+DateTime.Now);
       
        //READ FEATURE FILE INTO HASHSET
        ReadFeatureFileIntoHashSet();

        //READ FEATURES_TO_DELETE FILE AND DELETE GIVEN FEATURES FROM HASHSET
        ReadFeatureDeleteAndRemoveExisted();

        // WRITE RESULT OF HASHSET TO NEW FILE.
        WriteResultToNewFile();

        Console.WriteLine("Finished {0}, Press Any Key to Exit",DateTime.Now);
        Console.ReadLine();
       
        }

        private static void WriteResultToNewFile()
        {
            using (StreamWriter sr = new StreamWriter(resultFileName)){

                foreach (var item in featuresHashSet.AsEnumerable()){
                    sr.WriteLine(item);
                }
            }
        }

        private static void ReadFeatureDeleteAndRemoveExisted()
        {
            using (var fileStream = File.OpenRead(featureDeleteFileName)){
                using (var streamReader =
                    new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)){
                    
                    while ((currentDeletion = streamReader.ReadLine()) != null){
                        //Remove entry from feature if UDB match with Feature Delete
                        featuresHashSet.RemoveWhere(UDBExistAsSubstring);
                    }
                }
            }
        }
        
        //Check if UDB exist as substring in string
        private static bool UDBExistAsSubstring(string obj)
        {
            //Get the first 8 digit of hashset string (UDB No)
            //And check if it equals to Feature to Delete
            return obj.Substring(0, 8) == currentDeletion;
        }

        //Read feature file into hashset
        private static void ReadFeatureFileIntoHashSet()
        {
            using (var fileStream = File.OpenRead(featureFileName)){

                using (var streamReader =
                    new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)){
                    string line;
                    while ((line = streamReader.ReadLine()) != null){
                        featuresHashSet.Add(line);
                    }
                }
            }
        }
    }
}
