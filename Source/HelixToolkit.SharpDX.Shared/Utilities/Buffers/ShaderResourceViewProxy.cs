using SharpDX;
using SharpDX.Direct3D11;
using System.Threading;

#if !NETFX_CORE
namespace HelixToolkit.Wpf.SharpDX.Utilities
#else
namespace HelixToolkit.UWP.Utilities
#endif
{
    /// <summary>
    /// A proxy container to handle view resources
    /// </summary>
    public sealed class ShaderResourceViewProxy : ReferenceCountDisposeObject
    {
        /// <summary>
        /// Gets the texture view.
        /// </summary>
        /// <value>
        /// The texture view.
        /// </value>
        public ShaderResourceView TextureView { get { return textureView; } }
        private ShaderResourceView textureView;
        /// <summary>
        /// Gets the depth stencil view.
        /// </summary>
        /// <value>
        /// The depth stencil view.
        /// </value>
        public DepthStencilView DepthStencilView { get { return depthStencilView; } }
        private DepthStencilView depthStencilView;
        /// <summary>
        /// Gets the render target view.
        /// </summary>
        /// <value>
        /// The render target view.
        /// </value>
        public RenderTargetView RenderTargetView { get { return renderTargetView; } }
        private RenderTargetView renderTargetView;
        /// <summary>
        /// Gets the resource.
        /// </summary>
        /// <value>
        /// The resource.
        /// </value>
        public Resource Resource { get { return resource; } }
        private Resource resource;

        private readonly Device device;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceViewProxy"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public ShaderResourceViewProxy(Device device) { this.device = device; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceViewProxy"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="textureDesc">The texture desc.</param>
        public ShaderResourceViewProxy(Device device, Texture1DDescription textureDesc) : this(device)
        {
            resource = Collect(new Texture1D(device, textureDesc));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceViewProxy"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="textureDesc">The texture desc.</param>
        public ShaderResourceViewProxy(Device device, Texture2DDescription textureDesc) : this(device)
        {
            resource = Collect(new Texture2D(device, textureDesc));
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceViewProxy"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="textureDesc">The texture desc.</param>
        public ShaderResourceViewProxy(Device device, Texture3DDescription textureDesc) : this(device)
        {
            resource = Collect(new Texture3D(device, textureDesc));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="device"></param>
        /// <param name="resource"></param>
        public ShaderResourceViewProxy(Device device, Resource resource) : this(device)
        {
            this.resource = Collect(resource);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderResourceViewProxy"/> class.
        /// </summary>
        /// <param name="view">The view.</param>
        public ShaderResourceViewProxy(ShaderResourceView view) : this(view.Device)
        {
            textureView = Collect(view);
        }
        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="disableAutoGenMipMap">Disable auto mipmaps generation</param>
        public void CreateView(System.IO.Stream stream, bool disableAutoGenMipMap = false)
        {
            this.DisposeAndClear();
            if (stream != null && device != null)
            {
                textureView = Collect(TextureLoader.FromMemoryAsShaderResourceView(device, stream, disableAutoGenMipMap));
            }
        }
        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void CreateView(ShaderResourceViewDescription desc)
        {
            RemoveAndDispose(ref textureView);
            if (resource == null)
            {
                return;
            }
            textureView = Collect(new ShaderResourceView(device, resource, desc));
        }
        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void CreateView(DepthStencilViewDescription desc)
        {
            RemoveAndDispose(ref depthStencilView);
            if (resource == null)
            {
                return;
            }
            depthStencilView = Collect(new DepthStencilView(device, resource, desc));
        }
        /// <summary>
        /// Creates the view.
        /// </summary>
        /// <param name="desc">The desc.</param>
        public void CreateView(RenderTargetViewDescription desc)
        {
            RemoveAndDispose(ref renderTargetView);
            if (resource == null)
            {
                return;
            }
            renderTargetView = Collect(new RenderTargetView(device, resource, desc));
        }
        /// <summary>
        /// Creates the view.
        /// </summary>
        public void CreateTextureView()
        {
            RemoveAndDispose(ref textureView);
            if (resource == null)
            {
                return;
            }
            textureView = Collect(new ShaderResourceView(device, resource));
        }
        /// <summary>
        /// Creates the render target.
        /// </summary>
        public void CreateRenderTargetView()
        {
            RemoveAndDispose(ref renderTargetView);
            if (resource == null)
            {
                return;
            }
            renderTargetView = Collect(new RenderTargetView(device, resource));
        }

        public void CreateDepthStencilView()
        {
            RemoveAndDispose(ref depthStencilView);
            if (resource == null)
            {
                return;
            }
            depthStencilView = Collect(new DepthStencilView(device, resource));
        }


        /// <summary>
        /// Creates the 1D texture view from data array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="format">The pixel format.</param>
        /// <param name="createSRV">if set to <c>true</c> [create SRV].</param>
        public void CreateView<T>(T[] array, global::SharpDX.DXGI.Format format, bool createSRV = true) where T : struct
        {
            this.DisposeAndClear();
            resource = Collect(global::SharpDX.Toolkit.Graphics.Texture1D.New(device, array.Length, format, array));
            if (createSRV)
            {
                textureView = Collect(new ShaderResourceView(device, resource));
            }
        }
        /// <summary>
        /// Creates the 2D texture view from data array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="format">The format.</param>
        /// <param name="mipCount">The mipCount. Default = 0 Auto</param>
        /// <param name="createSRV">if set to <c>true</c> [create SRV].</param>
        public void CreateView<T>(T[] array, int width, int height, global::SharpDX.DXGI.Format format, int mipCount = 0, bool createSRV = true) where T : struct
        {
            this.DisposeAndClear();
            resource = Collect(global::SharpDX.Toolkit.Graphics.Texture2D.New(device, width, height, mipCount, format));
            if (createSRV)
            {
                textureView = Collect(new ShaderResourceView(device, resource));
            }
        }
        /// <summary>
        /// Creates the 1D texture view from color array.
        /// </summary>
        /// <param name="array">The array.</param>
        public void CreateViewFromColorArray(Color4[] array)
        {
            CreateView(array, global::SharpDX.DXGI.Format.R32G32B32A32_Float);
        }
        /// <summary>
        /// Creates the 2D texture view from color array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mipCount">The mipCount. Default = 0 Auto</param>
        /// <param name="createSRV"></param>
        public void CreateViewFromColorArray(Color4[] array, int width, int height, int mipCount = 0, bool createSRV = true)
        {
            CreateView(array, width, height, global::SharpDX.DXGI.Format.R32G32B32A32_Float, mipCount, createSRV);
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="ShaderResourceViewProxy"/> to <see cref="ShaderResourceView"/>.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator ShaderResourceView(ShaderResourceViewProxy proxy)
        {
            return proxy?.textureView;
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="ShaderResourceViewProxy"/> to <see cref="DepthStencilView"/>.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator DepthStencilView(ShaderResourceViewProxy proxy)
        {
            return proxy?.depthStencilView;
        }
        /// <summary>
        /// Performs an implicit conversion from <see cref="ShaderResourceViewProxy"/> to <see cref="RenderTargetView"/>.
        /// </summary>
        /// <param name="proxy">The proxy.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator RenderTargetView(ShaderResourceViewProxy proxy)
        {
            return proxy?.renderTargetView;
        }
    }
}
