/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

// Copyright (C) 2015 Google, Inc.
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
using System.Collections.Generic;

using GoogleMobileAds.Common;
using UnityEngine;

namespace GoogleMobileAds.Api
{
    public enum NativeAdType
    {
        CustomTemplate = 0,
    }

    public class AdLoader
    {
        private IAdLoaderClient adLoaderClient;

        private AdLoader(Builder builder)
        {
            this.AdUnitId = string.Copy(builder.AdUnitId);
            this.CustomNativeTemplateClickHandlers =
                    new Dictionary<string, Action<CustomNativeTemplateAd, string>>(
                    builder.CustomNativeTemplateClickHandlers);
            this.TemplateIds = new HashSet<string>(builder.TemplateIds);
            this.AdTypes = new HashSet<NativeAdType>(builder.AdTypes);
            this.adLoaderClient = GoogleMobileAdsClientFactory.BuildAdLoaderClient(this);

            this.adLoaderClient.OnCustomNativeTemplateAdLoaded += (sender, args) =>
                {
                    if(this.OnCustomNativeTemplateAdLoaded != null)
                    {
                        this.OnCustomNativeTemplateAdLoaded(this, args);
                    }
                };

            this.adLoaderClient.OnAdFailedToLoad += (sender, args) =>
                {
                    if(this.OnAdFailedToLoad != null)
                    {
                        this.OnAdFailedToLoad(this, args);
                    }
                };
        }

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

        public Dictionary<string, Action<CustomNativeTemplateAd, string>>
                CustomNativeTemplateClickHandlers
        {
            get; private set;
        }

        public string AdUnitId { get; private set; }

        public HashSet<NativeAdType> AdTypes { get; private set; }

        public HashSet<string> TemplateIds { get; private set; }

        public void LoadAd(AdRequest request)
        {
            this.adLoaderClient.LoadAd(request);
        }

        public class Builder
        {
            public Builder(string adUnitId)
            {
                this.AdUnitId = adUnitId;
                this.AdTypes = new HashSet<NativeAdType>();
                this.TemplateIds = new HashSet<string>();
                this.CustomNativeTemplateClickHandlers =
                        new Dictionary<string, Action<CustomNativeTemplateAd, string>>();
            }

            internal string AdUnitId { get; private set; }

            internal HashSet<NativeAdType> AdTypes { get; private set; }

            internal HashSet<string> TemplateIds { get; private set; }

            internal Dictionary<string, Action<CustomNativeTemplateAd, string>>
                    CustomNativeTemplateClickHandlers
            {
                get; private set;
            }

            public Builder ForCustomNativeAd(string templateId)
            {
                this.TemplateIds.Add(templateId);
                this.AdTypes.Add(NativeAdType.CustomTemplate);
                return this;
            }

            public Builder ForCustomNativeAd(
                    string templateId,
                    Action<CustomNativeTemplateAd, string> callback)
            {
                this.TemplateIds.Add(templateId);
                this.CustomNativeTemplateClickHandlers[templateId] = callback;
                this.AdTypes.Add(NativeAdType.CustomTemplate);
                return this;
            }

            public AdLoader Build()
            {
                return new AdLoader(this);
            }
        }
    }
}
