using System.Collections.Generic;

public enum ESound
{
    NullSound,
}

//ÿ����Ƶ��ӦΨһ��ESound��String�������Ǽ�ת��ʱʹ�ô����еķ���
public static class ESoundTranslator
{
    //������ö��˳��һ��
    private static string[] names = new string[] { "" };
    private static readonly Dictionary<string, ESound> eSoundDict = new Dictionary<string, ESound>();
    private static readonly Dictionary<ESound, string> nameDict = new Dictionary<ESound, string>();

    static ESoundTranslator()
    {
        for (int i = 0; i < names.Length; i++)
        {
            nameDict.Add((ESound)i, names[i]);
            eSoundDict.Add(names[i], (ESound)i);
        }
    }

    public static ESound ToESound(this string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return ESound.NullSound;
        if (eSoundDict.ContainsKey(name))
            return eSoundDict[name];
        return ESound.NullSound;
    }

    public static string ToName(this ESound eSound)
    {
        if (nameDict.ContainsKey(eSound))
            return nameDict[eSound];
        return "";
    }

}
