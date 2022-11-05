namespace MyTimer
{
    /// <summary>
    /// ���ֻ�Ч��
    /// </summary>
    public class TypeWriter : Timer<string, StringLerp>
    {
        /// <param name="text">�ı�</param>
        /// <param name="letterPerSecond">ÿ�����ַ���</param>
        /// <param name="start">�Ƿ�������ʱ��</param>
        public void Initialize(string text, float letterPerSecond, bool start = true)
        {
            base.Initialize("", text, text.Length / letterPerSecond, start);
        }
    }

    /// <summary>
    /// ΪTypeWriter�ṩ���ӽ�����ͣ�ٵĹ���
    /// </summary>
    public class TypeWriterExtend
    {
        public const string SentenceSeparator = "?!��������";

        private TypeWriter typeWriter;
        private string separator;
        private float interval;
        private readonly TimerOnly timer;

        private string target;

        private string text;
        private string Text
        {
            set
            {
                if (text != value)
                {
                    text = value;
                    if (value.Length > 1 && separator.IndexOf(target[value.Length - 1]) != -1)
                    {
                        //�����ظ��ָ��������ʱ
                        if (target.Length > value.Length)
                        {
                            if (separator.IndexOf(target[value.Length]) == -1)
                            {
                                typeWriter.Paused = true;
                                timer.Initialize(interval);
                            }
                        }
                        else
                        {
                            typeWriter.Paused = true;
                            timer.Initialize(interval);
                        }
                    }
                }
            }
        }

        public TypeWriterExtend()
        {
            timer = new TimerOnly();
            timer.Complete += AfterDelay;
        }

        /// <param name="typeWriter">Ҫ���Ƶ�TypeWriter</param>
        /// <param name="interval">ÿ�仰�������ͣ��ʱ��</param>
        /// <param name="separator">���б�־���ӽ����ı�־��Ϊ����ʹ��Ĭ�ϵĽ�����־</param>
        public void Initialize(TypeWriter typeWriter, float interval = 0.5f, string separator = null)
        {
            Dispose();
            this.typeWriter = typeWriter;
            this.separator = separator ?? SentenceSeparator;
            this.interval = interval;
            this.typeWriter.Tick += OnTick;
            this.typeWriter.Resume += OnResume;
        }

        private void OnResume(string _)
        {
            target = typeWriter.Target;
        }

        private void OnTick(string current)
        {
            Text = typeWriter.Current;
        }

        private void Dispose()
        {
            if (typeWriter != null)
            {
                typeWriter.Paused = true;
                typeWriter.Tick -= OnTick;
            }
        }

        private void AfterDelay(float _)
        {
            typeWriter.Paused = false;
        }
    }
}
