// ReSharper disable CheckNamespace

using System;
using System.Collections.Generic;
using System.Linq;

namespace AppodealInc.Mediation.DependencyManager.Editor
{
    internal partial class PluginConfigResponseModel
    {
        public List<List<Sdk>> GetGroupedSdks(SdkCategory category)
        {
            var output = new List<List<Sdk>>();

            var sdks = new List<Sdk>();
            sdks.AddRange(ios?.sdks?
                .Where(sdk => sdk.category == category && sdk.requirement != Requirement.Deprecated) ?? new List<Sdk>());
            sdks.AddRange(android?.sdks?
                .Where(sdk => sdk.category == category && sdk.requirement != Requirement.Deprecated) ?? new List<Sdk>());

            var query = sdks.GroupBy(sdk => sdk.status).OrderBy(gr => gr.First().name);

            foreach (var gr in query)
            {
                var sdksGroup = new List<Sdk>();
                sdksGroup.AddRange(gr);

                output.Add(sdksGroup);
            }

            return output;
        }

        public List<Sdk> GetMergedSdks()
        {
            var sdks = new List<Sdk>();
            if (ios?.sdks != null) sdks.AddRange(ios.sdks);
            if (android?.sdks != null) sdks.AddRange(android.sdks);
            return sdks;
        }

        public List<AdType> GetSupportedAdTypes(string sdkId)
        {
            if (String.IsNullOrWhiteSpace(sdkId)) return new List<AdType>();

            var output = new List<AdType>();

            ios?.adapters?
                .Where(adapter => adapter.status.EndsWith(sdkId))
                .Select(adapter => adapter.ad_types)
                .ToList()
                .ForEach(adapter => output.AddRange(adapter));

            android?.adapters?
                .Where(adapter => adapter.status.EndsWith(sdkId))
                .Select(adapter => adapter.ad_types)
                .ToList()
                .ForEach(adapter => output.AddRange(adapter));

            return output.Distinct().ToList();
        }

        public List<Sdk> GetSupportedMediations(string sdkId)
        {
            if (String.IsNullOrWhiteSpace(sdkId)) return new List<Sdk>();

            const char adapterPartsSeparator = '_';

            var output = new List<Sdk>();
            var mediationIds = new List<string>();

            if (ios?.adapters != null)
            {
                mediationIds.AddRange(ios.adapters
                    .Where(adapter => adapter.status.EndsWith(sdkId) && adapter.status.Contains(adapterPartsSeparator))
                    .Select(adapter => adapter.status.Split(adapterPartsSeparator)[0]));
            }

            if (android?.adapters != null)
            {
                mediationIds.AddRange(android.adapters
                    .Where(adapter => adapter.status.EndsWith(sdkId) && adapter.status.Contains(adapterPartsSeparator))
                    .Select(adapter => adapter.status.Split(adapterPartsSeparator)[0]));
            }

            mediationIds = mediationIds.Distinct().ToList();

            if (ios?.sdks != null)
            {
                output.AddRange(ios.sdks
                    .Where(sdk => sdk.category == SdkCategory.Mediation && mediationIds.Contains(sdk.status)));
            }

            if (android?.sdks != null)
            {
                output.AddRange(android.sdks
                    .Where(sdk => sdk.category == SdkCategory.Mediation && mediationIds.Contains(sdk.status)));
            }

            return output.GroupBy(sdk => sdk.status).Select(gr => gr.First()).OrderByDescending(sdk => sdk.name).ToList();
        }
    }
}
