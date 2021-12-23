
using EsercitazioneWeek4.ConsoleApp;

Console.WriteLine("*****Gestione Spese*****");

bool quit = false;
do
{
    Console.WriteLine($"-------Menu--------");
    Console.WriteLine();
    Console.WriteLine("[ 1 ] - Inserire una nuova Spesa\n");
    Console.WriteLine("[ 2 ] - Approva Spesa\n");
    Console.WriteLine("[ 3 ] - Cancella Spesa Esistente\n");
    Console.WriteLine("[ 4 ] - Mostra elenco Spese Approvate\n");
    Console.WriteLine("[ 5 ] - Mostra l'elenco delle spese di uno specifico utente\n");
    Console.WriteLine("[ 6 ] - Mostra il totale delle spese per categoria\n");
    Console.WriteLine("[ q ] - QUIT\n");


    // scelta utente
    Console.Write("> ");
    string scelta = Console.ReadLine();
    Console.WriteLine();

    switch (scelta)
    {
        case "1":
            ConnectedMode.InserisciSpesa();         
            break;
        case "2":
            ConnectedMode.ApprovaSpesa();
            break;
        case "3":
            ConnectedMode.CancellaSpesa();            
            break;
        case "4":
            ConnectedMode.MostraSpeseApprovate();
            break;
        case "5":
            Console.WriteLine("Inserire il nome utente: ");
            string utente=Console.ReadLine();
            ConnectedMode.MostraSpeseUtente(utente);
            break;
        case "6":
            //ConnectedMode.TotaleSpeseCategoriaSceltaDallUtente();
            ConnectedMode.TotaleSpeseCategorie();
            break;
        case "q":
            quit = true;
            break;
        default:
            Console.WriteLine("Comando sconosciuto.");
            break;
    }

} while (!quit);

