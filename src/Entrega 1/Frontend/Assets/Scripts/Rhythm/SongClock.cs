using UnityEngine;

// Relógio da fase: único ponto de tempo do sistema de ritmo.
// REGRA: nenhum script usa Time.time para tempo de nota ou julgamento. Todos leem SongClock.Now.
//
// IMPLEMENTAÇÃO PROVISÓRIA: baseada em Time.timeAsDouble.
// No futuro, quando houver música, só o corpo de "Now" muda para
// AudioSettings.dspTime - startTime. Nenhum outro script precisa ser alterado.
//
// Obs.: Time.timeAsDouble respeita Time.timeScale, então pausar o jogo com
// Time.timeScale = 0 também pausa este relógio.
public static class SongClock
{
    private static double startTime;
    private static bool running;

    // Segundos desde o início da fase. Antes de Begin() devolve 0.
    public static double Now
    {
        get
        {
            if (!running) return 0.0;

            // TROCAR AQUI no futuro: return AudioSettings.dspTime - startTime;
            return Time.timeAsDouble - startTime;
        }
    }

    public static bool IsRunning => running;

    // Zera o relógio. Chamar uma vez no começo de cada fase.
    public static void Begin()
    {
        startTime = Time.timeAsDouble;
        running = true;
    }

    // Para o relógio (por exemplo, no fim da fase). Now volta a ser 0.
    public static void Stop()
    {
        running = false;
    }

    // Garante que o estado antigo não sobra entre um Play e outro no Editor
    // (caso o "Domain Reload" esteja desligado nas Project Settings).
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStatics()
    {
        startTime = 0.0;
        running = false;
    }
}