using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;

public class RateLimitTracker
{
    private static Dictionary<string, DateTime> RequestTracker = new Dictionary<string, DateTime>();
    private static object LockObject = new object();
    private static TimeSpan RequestLimitDuration = TimeSpan.FromMilliseconds(100);

    public static bool IsRateLimited(string ipAddress)
    {
        lock (LockObject)
        {
            if (RequestTracker.ContainsKey(ipAddress))
            {
                var lastRequestTime = RequestTracker[ipAddress];
                if (DateTime.Now - lastRequestTime < RequestLimitDuration)
                {
                    return true; // El límite de velocidad se ha alcanzado
                }
            }
            RequestTracker[ipAddress] = DateTime.Now; // Registra la dirección IP y la hora actual
            Console.WriteLine($"Dirección IP {ipAddress} agregada a RequestTracker en {DateTime.Now}"); // Registro
            return false; // No se ha alcanzado el límite de velocidad
        }
    }
}

class Program
{
    public static void Main()
    {
        // Simula solicitudes desde diferentes direcciones IP
        string ipAddress1 = "192.168.0.1";
        string ipAddress2 = "192.168.0.2";

        // Realiza algunas solicitudes desde la primera dirección IP
        for (int i = 0; i < 10; i++)
        {
            if (RateLimitTracker.IsRateLimited(ipAddress1))
            {
                Console.WriteLine($"La dirección IP {ipAddress1} ha alcanzado el límite de velocidad.");
            }
            else
            {
                Console.WriteLine($"Solicitud exitosa desde {ipAddress1}.");
            }
        }

        // Realiza algunas solicitudes desde la segunda dirección IP
        for (int i = 0; i < 10; i++)
        {
            if (RateLimitTracker.IsRateLimited(ipAddress2))
            {
                Console.WriteLine($"La dirección IP {ipAddress2} ha alcanzado el límite de velocidad.");
            }
            else
            {
                Console.WriteLine($"Solicitud exitosa desde {ipAddress2}.");
            }
        }
        Console.ReadLine();

    }
}
