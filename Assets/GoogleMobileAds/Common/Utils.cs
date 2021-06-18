/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Copyright (C) 2016 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using UnityEngine;

namespace GoogleMobileAds.Common
{
    internal class Utils
    {
        public static Texture2D GetTexture2DFromByteArray(byte[] img)
        {
            // Create a texture. Texture size does not matter, since
            // LoadImage will replace with with incoming image size.
            Texture2D nativeAdTexture = new Texture2D(1, 1);
            if (!nativeAdTexture.LoadImage(img))
            {
                throw new InvalidOperationException(@"Could not load custom native template
                        image asset as texture");
            }
            return nativeAdTexture;
        }
    }
}

