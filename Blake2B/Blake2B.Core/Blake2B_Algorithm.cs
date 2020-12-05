using System;
using System.Text;

namespace Blake2B.Core
{
    public class Blake2B_Algorithm
    {
        private const int ROUNDS = 12;
        private const int BLOCK_BYTES = 128;
        private const int HASH_SIZE_IN_BYTES = 64;

        private const ulong IV0 = 0x6A09E667F3BCC908UL;
        private const ulong IV1 = 0xBB67AE8584CAA73BUL;
        private const ulong IV2 = 0x3C6EF372FE94F82BUL;
        private const ulong IV3 = 0xA54FF53A5F1D36F1UL;
        private const ulong IV4 = 0x510E527FADE682D1UL;
        private const ulong IV5 = 0x9B05688C2B3E6C1FUL;
        private const ulong IV6 = 0x1F83D9ABFB41BD6BUL;
        private const ulong IV7 = 0x5BE0CD19137E2179UL;

        private const int R1 = 32;
        private const int R2 = 24;
        private const int R3 = 16;
        private const int R4 = 63;

        private readonly ulong[] _m = new ulong[16];
        private readonly ulong[] _v = new ulong[16];
        private readonly ulong[] _state = new ulong[8];
        private readonly byte[] _buffer = new byte[BLOCK_BYTES];

        private int _bufferFilled;
        private ulong _counter0;
        private ulong _counter1;
        private ulong _flag0;
        private ulong _flag1;

        private readonly int[] _sigma = new int[ROUNDS * 16] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            14, 10, 4, 8, 9, 15, 13, 6, 1, 12, 0, 2, 11, 7, 5, 3,
            11, 8, 12, 0, 5, 2, 15, 13, 10, 14, 3, 6, 7, 1, 9, 4,
            7, 9, 3, 1, 13, 12, 11, 14, 2, 6, 5, 10, 4, 0, 15, 8,
            9, 0, 5, 7, 2, 4, 10, 15, 14, 1, 11, 12, 6, 8, 3, 13,
            2, 12, 6, 10, 0, 11, 8, 3, 4, 13, 7, 5, 15, 14, 1, 9,
            12, 5, 1, 15, 14, 13, 4, 10, 0, 7, 6, 3, 9, 2, 8, 11,
            13, 11, 7, 14, 12, 1, 3, 9, 5, 0, 15, 4, 8, 6, 2, 10,
            6, 15, 14, 9, 11, 3, 0, 8, 12, 2, 13, 7, 1, 4, 10, 5,
            10, 2, 8, 4, 7, 6, 1, 5, 15, 11, 9, 14, 3, 12, 13, 0,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15,
            14, 10, 4, 8, 9, 15, 13, 6, 1, 12, 0, 2, 11, 7, 5, 3
        };

        public string ComputeHash(string input)
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hash = ComputeHash(inputBytes);
            return BitConverter.ToString(hash).Replace("-", " ");
        }

        public byte[] ComputeHash(byte[] input)
        {
            Initialize();
            HashCore(input);
            return HashFinal();
        }

        private void Initialize()
        {
            ClearHash();

            _state[0] = IV0;
            _state[1] = IV1;
            _state[2] = IV2;
            _state[3] = IV3;
            _state[4] = IV4;
            _state[5] = IV5;
            _state[6] = IV6;
            _state[7] = IV7;

            var c = new ulong[8];
            c[0] |= HASH_SIZE_IN_BYTES;
            c[0] |= 1 << 16;
            c[0] |= 1 << 24;
            c[0] |= 0 << 32;

            for (var i = 0; i < 8; i++)
                _state[i] ^= c[i];
        }

        private void ClearHash()
        {
            _counter0 = 0UL;
            _counter1 = 0UL;
            _flag0 = 0UL;
            _flag1 = 0UL;
            _bufferFilled = 0;

            for (var i = 0; i < BLOCK_BYTES; ++i)
                _buffer[i] = 0x00;

            for (var i = 0; i < 8; ++i)
                _state[i] = 0UL;

            for (var i = 0; i < 16; ++i)
                _m[i] = 0UL;
        }

        private void HashCore(byte[] input)
        {
            var length = input.Length;
            var offset = 0;

            while (0 < length)
            {
                var bytesToFill = Math.Min(length, BLOCK_BYTES - _bufferFilled);
                Buffer.BlockCopy(input, 0, _buffer, _bufferFilled, bytesToFill);

                _bufferFilled += bytesToFill;
                offset += bytesToFill;
                length -= bytesToFill;

                if (_bufferFilled == BLOCK_BYTES)
                {
                    IncrementCounter(BLOCK_BYTES);

                    if (BitConverter.IsLittleEndian)
                        Buffer.BlockCopy(_buffer, 0, _m, 0, BLOCK_BYTES);
                    else
                    {
                        for (int i = 0; i < BLOCK_BYTES / 8; i++)
                            _m[i] = BytesToUInt64(_buffer, i << 3);
                    }

                    Compress();
                    _bufferFilled = 0;
                }
            }
        }

        private byte[] HashFinal()
        {
            var hash = new byte[HASH_SIZE_IN_BYTES];

            IncrementCounter((ulong)_bufferFilled);
            SetLastBlock();

            for (int i = _bufferFilled; i < BLOCK_BYTES; i++)
                _buffer[i] = 0x00;

            if (BitConverter.IsLittleEndian)
                Buffer.BlockCopy(_buffer, 0, _m, 0, BLOCK_BYTES);
            else
                for (int i = 0; i < BLOCK_BYTES / 8; ++i)
                    _m[i] = BytesToUInt64(_buffer, i << 3);

            Compress();

            if (BitConverter.IsLittleEndian)
                Buffer.BlockCopy(_state, 0, hash, 0, HASH_SIZE_IN_BYTES);
            else
                for (int i = 0; i < HASH_SIZE_IN_BYTES / 4; ++i)
                    UInt64ToBytes(_state[i], hash, i << 3);

            return hash;
        }

        private void Compress()
        {
            for (int i = 0; i < 8; i++)
                _v[i] = _state[i];

            _v[8] = IV0;
            _v[9] = IV1;
            _v[10] = IV2;
            _v[11] = IV3;
            _v[12] = IV4 ^ _counter0;
            _v[13] = IV5 ^ _counter1;
            _v[14] = IV6 ^ _flag0;
            _v[15] = IV7 ^ _flag1;

            for (int r = 0; r < 12; ++r)
            {
                Mixing(0, 4, 8, 12, r, 0);
                Mixing(1, 5, 9, 13, r, 2);
                Mixing(2, 6, 10, 14, r, 4);
                Mixing(3, 7, 11, 15, r, 6);
                Mixing(3, 4, 9, 14, r, 14);
                Mixing(2, 7, 8, 13, r, 12);
                Mixing(0, 5, 10, 15, r, 8);
                Mixing(1, 6, 11, 12, r, 10);
            }

            for (int i = 0; i < 8; ++i)
                _state[i] ^= _v[i] ^ _v[i + 8];
        }

        private void Mixing(int a, int b, int c, int d, int r, int i)
        {
            var p = (r << 4) + i;
            var p0 = _sigma[p];
            var p1 = _sigma[p + 1];

            _v[a] += _v[b] + _m[p0];
            _v[d] = RotateRight(_v[d] ^ _v[a], R1);
            _v[c] += _v[d];
            _v[b] = RotateRight(_v[b] ^ _v[c], R2);
            _v[a] += _v[b] + _m[p1];
            _v[d] = RotateRight(_v[d] ^ _v[a], R3);
            _v[c] += _v[d];
            _v[b] = RotateRight(_v[b] ^ _v[c], R4);
        }

        protected void IncrementCounter(ulong incrementor)
        {
            _counter0 += incrementor;

            if (_counter0 == 0)
                _counter1++;
        }

        protected void SetLastBlock()
        {
            if (_flag1 == ulong.MaxValue)
                _flag1 = ulong.MaxValue;

            _flag0 = ulong.MaxValue;
        }

        private ulong RotateRight(ulong value, int bits)
        {
            return value >> bits | value << 64 - bits;
        }

        private ulong BytesToUInt64(byte[] buffer, int offset)
        {
            return
                (ulong)buffer[offset + 7] << 7 * 8 |
                (ulong)buffer[offset + 6] << 6 * 8 |
                (ulong)buffer[offset + 5] << 5 * 8 |
                (ulong)buffer[offset + 4] << 4 * 8 |
                (ulong)buffer[offset + 3] << 3 * 8 |
                (ulong)buffer[offset + 2] << 2 * 8 |
                (ulong)buffer[offset + 1] << 1 * 8 |
                buffer[offset];
        }

        private void UInt64ToBytes(ulong value, byte[] buffer, int offset)
        {
            buffer[offset + 7] = (byte)(value >> 7 * 8);
            buffer[offset + 6] = (byte)(value >> 6 * 8);
            buffer[offset + 5] = (byte)(value >> 5 * 8);
            buffer[offset + 4] = (byte)(value >> 4 * 8);
            buffer[offset + 3] = (byte)(value >> 3 * 8);
            buffer[offset + 2] = (byte)(value >> 2 * 8);
            buffer[offset + 1] = (byte)(value >> 1 * 8);
            buffer[offset] = (byte)value;
        }
    }
}
