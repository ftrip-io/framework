using System.Threading.Tasks;
using System;
using System.IO;
using System.Threading;

namespace ftrip.io.framework.Metrics
{
    public class ContentLengthTrackingStream : Stream
    {
        private readonly Stream _inner;
        private long _contentLength;

        public ContentLengthTrackingStream(Stream inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
            _contentLength = 0;
        }

        public override long Length => _contentLength;

        public override bool CanRead => _inner.CanRead;

        public override bool CanSeek => _inner.CanSeek;

        public override bool CanWrite => _inner.CanWrite;

        public override bool CanTimeout => _inner.CanTimeout;

        public override long Position
        {
            get => _inner.Position;
            set => _inner.Position = value;
        }

        public override int ReadTimeout
        {
            get => _inner.ReadTimeout;
            set => _inner.ReadTimeout = value;
        }

        public override int WriteTimeout
        {
            get => _inner.WriteTimeout;
            set => _inner.WriteTimeout = value;
        }

        public override void Flush() => _inner.Flush();

        public override Task FlushAsync(CancellationToken cancellationToken) => _inner.FlushAsync(cancellationToken);

        public override int Read(byte[] buffer, int offset, int count)
            => _inner.Read(buffer, offset, count);

        public async override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
            => await _inner.ReadAsync(buffer, offset, count, cancellationToken);

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
            => _inner.BeginRead(buffer, offset, count, callback, state);

        public override int EndRead(IAsyncResult asyncResult)
            => asyncResult is Task<int> task ? task.GetAwaiter().GetResult() : _inner.EndRead(asyncResult);

        public override long Seek(long offset, SeekOrigin origin) => _inner.Seek(offset, origin);

        public override void SetLength(long value) => _inner.SetLength(value);

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            _contentLength += count;
            return _inner.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            _contentLength += count;
            _inner.Write(buffer, offset, count);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            _contentLength += count;
            return _inner.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            _contentLength++;
            _inner.WriteByte(value);
        }

        public override void EndWrite(IAsyncResult asyncResult) => _inner.EndWrite(asyncResult);

        protected override void Dispose(bool disposing)
        {
            if (disposing) _inner.Dispose();
        }
    }
}
