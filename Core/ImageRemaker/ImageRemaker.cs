using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageRemaker
{
    public static class ImageRemake
    {
        static ComputeShader shader => ReturnShader.returnshader();

        struct ThreadSize
        {
            public uint x;
            public uint y;
            public uint z;

            public ThreadSize(uint x, uint y, uint z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        }

        #region BaseMethods

        static RenderTexture CreateRenderTexture(Texture2D texture)
        {
            if (!SystemInfo.supportsComputeShaders)
            {
                Debug.LogError("Comppute Shader is not support.");
            }
            var result = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.ARGB32);
            result.enableRandomWrite = true;
            result.Create();
            return result;
        }

        static void DispatchShader(int index, Texture2D texture, RenderTexture result, ComputeShader shader)
        {
            ThreadSize threadSize = new ThreadSize();
            shader.GetKernelThreadGroupSizes(index, out threadSize.x, out threadSize.y, out threadSize.z);
            shader.SetTexture(index, "Texture", texture);
            shader.SetTexture(index, "Result", result);
            shader.Dispatch(index, texture.width / (int)threadSize.x, texture.height / (int)threadSize.y, (int)threadSize.z);

        }

        static void DispatchShader(int index, Texture2D texture, Texture2D texture2, RenderTexture result, ComputeShader shader)
        {
            ThreadSize threadSize = new ThreadSize();
            shader.GetKernelThreadGroupSizes(index, out threadSize.x, out threadSize.y, out threadSize.z);
            shader.SetTexture(index, "Texture", texture);
            shader.SetTexture(index, "Texture2", texture2);
            shader.SetTexture(index, "Result", result);
            shader.Dispatch(index, texture.width / (int)threadSize.x, texture.height / (int)threadSize.y, (int)threadSize.z);

        }

        static void DispatchShader(int index, Texture2D texture, Texture2D texture2, Texture2D texture3, RenderTexture result, ComputeShader shader)
        {
            ThreadSize threadSize = new ThreadSize();
            shader.GetKernelThreadGroupSizes(index, out threadSize.x, out threadSize.y, out threadSize.z);
            shader.SetTexture(index, "Texture", texture);
            shader.SetTexture(index, "Texture2", texture2);
            shader.SetTexture(index, "Texture3", texture3);
            shader.SetTexture(index, "Result", result);
            shader.Dispatch(index, texture.width / (int)threadSize.x, texture.height / (int)threadSize.y, (int)threadSize.z);

        }

        static Texture2D MakeTexture(RenderTexture render)
        {
            Texture2D texture = new Texture2D(render.width, render.height, TextureFormat.ARGB32, false, false);
            RenderTexture.active = render;
            texture.ReadPixels(new Rect(0, 0, render.width, render.height), 0, 0);
            RenderTexture.active = null;

            texture.Apply();
            return texture;
        }

        #endregion

        public static Texture2D GrayScale(this Texture2D texture)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("GrayScale");

            #region Original


            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Inverse(this Texture2D texture)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("Inverse");

            #region Original


            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Monochrome(this Texture2D texture, float threshold)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("Monochrome");


            #region Original

            shader.SetFloat("MonochromeValue", threshold);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        } //range of threshold is 0～1

        public static Texture2D Blur(this Texture2D _texture, int repeat)
        {
            Texture2D filter(Texture2D texture)
            {
                var result = CreateRenderTexture(texture);

                var Index = shader.FindKernel("Blur");

                #region Original


                #endregion

                DispatchShader(Index, texture, result, shader);
                return MakeTexture(result);
            }
            Texture2D tex = _texture;
            for (int i = 1; i <= repeat; i++)
            {
                tex = filter(tex);
            }
            return tex;

        }

        public static Texture2D Blur(this Texture2D _texture, int repeat, int scale)
        {
            Texture2D filter(Texture2D texture)
            {
                var result = CreateRenderTexture(texture);

                var Index = shader.FindKernel("BlurScale");

                #region Original

                shader.SetInt("scale", scale);

                #endregion

                DispatchShader(Index, texture, result, shader);
                return MakeTexture(result);
            }
            Texture2D tex = _texture;
            for (int i = 1; i <= repeat; i++)
            {
                tex = filter(tex);
            }
            return tex;

        }

        public static Texture2D SetAlpha(this Texture2D texture, float alpha)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("SetAlpha");

            #region Original

            shader.SetFloat("alpha", alpha);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D OverColor(this Texture2D texture, Color color)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("OverColor");


            #region Original

            Vector4 _color = new Vector4(color.r, color.g, color.b, color.a);
            shader.SetVector("overcolor", _color);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D SetColor(this Texture2D texture, Color color)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("SetColor");


            #region Original
            Vector4 _color = new Vector4(color.r, color.g, color.b, color.a);
            shader.SetVector("setcolor", _color);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D ExpandBorder(this Texture2D texture, Color color, int repeat)
        {

            var Index = shader.FindKernel("ExpandBorder");
            #region Original

            Vector4 _color = new Vector4(color.r, color.g, color.b, color.a);
            shader.SetVector("borderColor", _color);

            #endregion

            Texture2D filter(Texture2D texture)
            {
                var result = CreateRenderTexture(texture);

                DispatchShader(Index, texture, result, shader);
                return MakeTexture(result);
            }
            Texture2D tex = texture;
            for (int i = 1; i <= repeat; i++)
            {
                tex = filter(tex);
            }
            return tex;

        }

        public static Texture2D AddColor(this Texture2D texture, Texture2D texture2, float blend = 1)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("PlusMinus");

            #region Original

            shader.SetFloat("blend", blend);

            #endregion

            DispatchShader(Index, texture, texture2, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D MinusColor(this Texture2D texture, Texture2D texture2, float blend = 1)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("PlusMinus");

            #region Original

            shader.SetFloat("blend", blend * -1);

            #endregion

            DispatchShader(Index, texture, texture2, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D MultipleColor(Texture2D texture, Texture2D texture2, float blend = 1)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("Multiple");

            #region Original

            shader.SetFloat("blend", blend);

            #endregion

            DispatchShader(Index, texture, texture2, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D BlackToAlpha(this Texture2D texture, float threshold)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("BlackToAlpha");

            #region Original

            shader.SetFloat("blackThreshold", threshold);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Clamp(this Texture2D texture, float max, float min , bool equal = false)
        {
            var result = CreateRenderTexture(texture);

            int Index = 0;
            if(equal == true) Index = shader.FindKernel("Clamp_Equal");
            else Index = shader.FindKernel("Clamp");


            #region Original

            shader.SetFloat("maxClamp", max);
            shader.SetFloat("minClamp", min);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D OverRide(Texture2D texture, Texture2D texture2, Texture2D mask, float blend = 1)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("OverRide");

            #region Original

            shader.SetFloat("blend", blend);

            #endregion

            DispatchShader(Index, texture, texture2, mask, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D HSL(this Texture2D texture, float hue, float saturation, float value)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("HSL");

            #region Original

            shader.SetFloat("hue", hue);
            shader.SetFloat("saturation", saturation);
            shader.SetFloat("value", value);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D BlurBrush(this Texture2D texture, Vector2Int position, int scale, int scope, int pow = 2)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("BlurBrush");

            #region Original

            int[] posi = new int[] { position.x, position.y };
            shader.SetInts("positionUV", posi);
            shader.SetInt("center", scope);
            shader.SetInt("powNum", pow);
            shader.SetInt("scale", scale);


            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Mirror(this Texture2D texture, bool x, bool y)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("Mirror");

            #region Original

            shader.SetBool("isMirrorX", x);
            shader.SetBool("isMirrorY", y);

            int[] size = new int[] { texture.width, texture.height };
            shader.SetInts("maxResolution", size);


            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Rotate(this Texture2D texture)
        {
            var result = CreateRenderTexture(texture);

            var Index = shader.FindKernel("Rotate");

            #region Original

            int[] size = new int[] { texture.width, texture.height };
            shader.SetInts("maxResolution", size);

            #endregion

            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }

        public static Texture2D Bound(this Texture2D texture, float perBound) { return Bound(texture, (int)(perBound* texture.height)); }
        public static Texture2D Bound(this Texture2D texture, float perHeight ,float perWidth) {
            return Bound(texture, (int)(perHeight * texture.height), (int)(perWidth * texture.width)); 
        }
        public static Texture2D Bound(this Texture2D texture, int bound) { return Bound(texture, bound, bound); }
        public static Texture2D Bound(this Texture2D texture, int height, int width)
        {
            var result = CreateRenderTexture(texture);
            var Index = shader.FindKernel("Bound");

            #region Original

            int[] size = new int[] { texture.width, texture.height };
            shader.SetInts("maxResolution", size);
            shader.SetInt("heightBound", height);
            shader.SetInt("widthBound", width);

            #endregion
            DispatchShader(Index, texture, result, shader);
            return MakeTexture(result);
        }
    }
}
