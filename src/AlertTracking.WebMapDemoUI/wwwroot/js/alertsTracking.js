const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/alert")
    .build();

hubConnection.start();

export async function getRegionsWithAlerts() {
    try {
        const regions = await new Promise((resolve) => {
            hubConnection.invoke("SendRegions");
            hubConnection.on('ReceiveRegions', function (regions) {
                const result = regions.map(region => ({
                    regionName: region.regionName,
                    isAlert: region.isAlert
                }));

                resolve(result);
            });
        });

        return regions;
    }
    catch (error) {
        console.error("Error fetching regions with alerts:", error);
        return []; 
    }
}