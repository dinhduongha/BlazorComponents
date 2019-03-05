﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RPedretti.Blazor.BingMap.Collections;
using RPedretti.Blazor.BingMap.Entities;
using RPedretti.Blazor.BingMap.Entities.Layer;
using RPedretti.Blazor.BingMap.Modules;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RPedretti.Blazor.BingMap
{
    public partial class BingMapBase : ComponentBase, IDisposable
    {
        #region Fields
        private const string _mapNamespace = "rpedrettiBlazorComponents.bingMaps.map";
        private BingMapEntityList _entities;
        private BingMapLayerList _layers;
        private ObservableCollection<IBingMapModule> _modules;
        private bool _shouldRender;
        private BingMapsViewConfig _viewConfig;
        private bool modulesLoaded;
        private DotNetObjectRef thisRef;

        #endregion Fields

        #region Methods

        private async Task AddItemAsync(BaseBingMapEntity baseBingMapEntity)
        {
            try
            {
                await JSRuntime.Current.InvokeAsync<object>($"{_mapNamespace}.addItem", Id, baseBingMapEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void EntitiesChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    AddItemAsync(_entities[e.NewIndex]);
                    break;

                case ListChangedType.ItemChanged:
                    UpdateItemAsync(_entities[e.NewIndex]);
                    break;
            }
        }

        private async void EntitiesRemoved(object sender, BaseBingMapEntity removed)
        {
            try
            {
                await JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.removeItem", Id, removed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void LayerRemoved(object sender, BingMapLayer removed)
        {
            try
            {
                await JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.removeLayer", Id, removed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async void LayersChanged(object sender, ListChangedEventArgs e)
        {
            try
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        await JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.addLayer", Id, _layers[e.NewIndex].Id);
                        break;
                    case ListChangedType.ItemChanged:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void ModulesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && modulesLoaded)
            {
                foreach (IBingMapModule item in e.NewItems)
                {
                    item.InitAsync(Id);
                }
            }
        }

        private async Task RemoveItemAsync(BaseBingMapEntity baseBingMapEntity)
        {
            try
            {
                await JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.removeItem", Id, baseBingMapEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private bool SetParameter<T>(ref T prop, T value, Action onChange = null)
        {
            if (EqualityComparer<T>.Default.Equals(prop, value))
            {
                return false;
            }

            prop = value;
            onChange?.Invoke();

            return true;
        }

        private async Task UpdateItemAsync(BaseBingMapEntity baseBingMapEntity)
        {
            try
            {
                await JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.updateItem", Id, baseBingMapEntity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void UpdateView(BingMapsViewConfig viewConfig)
        {
            JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.updateView", Id, viewConfig);
        }

        public async Task<Geocoordinate> GetCenterAsync()
        {
            return await JSRuntime.Current.InvokeAsync<Geocoordinate>($"{_mapNamespace}.getCenter", Id);
        }

        public async Task<LocationRectangle> GetBoundsAsync()
        {
            return await JSRuntime.Current.InvokeAsync<LocationRectangle>($"{_mapNamespace}.getBounds", Id);
        }

        #endregion Methods

        public BingMapBase()
        {
            thisRef = new DotNetObjectRef(this);
        }

        protected bool init;

        [Parameter]
        protected BingMapEntityList Entities
        {
            get => _entities;
            set
            {
                if (!EqualityComparer<BindingList<BaseBingMapEntity>>.Default.Equals(_entities, value))
                {
                    if (_entities != null)
                    {
                        _entities.ListChanged -= EntitiesChanged;
                        _entities.BeforeRemove -= EntitiesRemoved;
                        _entities.BeforeRemoveRange -= BeforeRemoveRange;
                        _entities.ListRangeChanged -= EntitiesRangeChanged;
                        ClearItems();
                    }

                    _entities = value;

                    if (_entities != null)
                    {
                        AddItems(0, Entities.Count);
                        _entities.ListChanged += EntitiesChanged;
                        _entities.BeforeRemove += EntitiesRemoved;
                        _entities.BeforeRemoveRange += BeforeRemoveRange;
                        _entities.ListRangeChanged += EntitiesRangeChanged;
                    }
                }
            }
        }

        private async void BeforeRemoveRange(object sender, IEnumerable<BaseBingMapEntity> e)
        {
            await JSRuntime.Current.InvokeAsync<object>($"{_mapNamespace}.removeItems", e);
        }

        private void EntitiesRangeChanged(object sender, RangeChangdEventArgs e)
        {
            switch (e.Type)
            {
                case RangeChangeType.Add:
                    AddItems(e.StartIndex, e.Ammount);
                    break;
            }
        }

        private async Task ClearItems()
        {
            await JSRuntime.Current.InvokeAsync<object>($"{_mapNamespace}.clearItems");
        }

        private async Task AddItems(int start, int count)
        {
            await JSRuntime.Current.InvokeAsync<object>($"{_mapNamespace}.addItems", Id, Entities.Skip(start).Take(count));
        }

        [Parameter] protected string Id { get; set; } = $"bing-map-{Guid.NewGuid().ToString().Replace("-", "")}";

        [Parameter]
        protected BingMapLayerList Layers
        {
            get => _layers;
            set
            {
                if (_layers != null)
                {
                    _layers.ListChanged -= LayersChanged;
                    _layers.BeforeRemove -= LayerRemoved;
                }

                _layers = value;

                if (_layers != null)
                {
                    _layers.ListChanged += LayersChanged;
                    _layers.BeforeRemove += LayerRemoved;
                }
            }
        }

        [Parameter] protected Func<Task> MapLoaded { get; set; }
        [Parameter] protected BingMapConfig MapsConfig { get; set; } = new BingMapConfig();

        [Parameter]
        protected ObservableCollection<IBingMapModule> Modules
        {
            get => _modules;
            set
            {
                if (_modules != null)
                {
                    _modules.CollectionChanged -= ModulesChanged;
                }

                _modules = value;

                if (_modules != null)
                {
                    _modules.CollectionChanged += ModulesChanged;
                }
            }
        }

        [Parameter]
        protected BingMapsViewConfig ViewConfig
        {
            get => _viewConfig;
            set
            {
                if (SetParameter(ref _viewConfig, value))
                {
                    UpdateView(value);
                }

                _shouldRender = true;
            }
        }

        protected override void OnAfterRender()
        {
            if (!init)
            {
                init = true;
                JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.getMap", thisRef, Id, MapsConfig);
            }

            _shouldRender = false;
        }

        protected override bool ShouldRender()
        {
            return _shouldRender;
        }

        public void Dispose()
        {
            Modules = null;
            Entities = null;
            Layers = null;
            MapLoaded = null;

            JSRuntime.Current.UntrackObjectRef(thisRef);
            JSRuntime.Current.InvokeAsync<object>("rpedrettiBlazorComponents.bingMaps.map.unloadMap", Id);
        }

        [JSInvokable]
        public async Task NotifyMapLoaded()
        {
            if (Modules != null)
            {
                foreach (var module in Modules)
                {
                    await module.InitAsync(Id);
                }
            }
            StateHasChanged();
            MapLoaded?.Invoke();
            modulesLoaded = true;
        }
    }
}
