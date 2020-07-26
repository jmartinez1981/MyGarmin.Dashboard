import React from "react";
import Grid from "@material-ui/core/Grid";
import Typography from "@material-ui/core/Typography";
import ChartRenderer from "./components/ChartRenderer";
import Dashboard from "./components/Dashboard";
import DashboardItem from "./components/DashboardItem";
const DashboardItems = [
  {
    id: 0,
    name: "Activities (This Year)",
    vizState: {
      query: {
        timeDimensions: [
          {
            dimension: "Activities.startdate",
            dateRange: "This year"
          }
        ],
        order: {},
        dimensions: [],
        measures: ["Activities.count"],
        filters: []
      },
      chartType: "number"
    }
  },
  {
    id: 1,
    name: "Activities (This Week)",
    vizState: {
      query: {
        measures: ["Activities.count"],
        timeDimensions: [
          {
            dimension: "Activities.startdate",
            dateRange: "This week"
          }
        ],
        order: {},
        filters: []
      },
      chartType: "number"
    }
  },
  {
    id: 2,
    name: "Running Distance (This Week)",
    vizState: {
      query: {
        measures: ["Activities.DistanceSum"],
        timeDimensions: [
          {
            dimension: "Activities.startdate",
            dateRange: "This week"
          }
        ],
        order: {},
        filters: []
      },
      chartType: "number"
    }
  },
  {
    id: 3,
    name: "Elevation Gain (This Week)",
    vizState: {
      query: {
        measures: ["Activities.ElevationGainSum"],
        timeDimensions: [
          {
            dimension: "Activities.startdate",
            dateRange: "This week"
          }
        ],
        order: {},
        filters: []
      },
      chartType: "number"
    }
  },
  {
    id: 4,
    name: "Summary Activies (This Week)",
    vizState: {
      query: {
        measures: [],
        timeDimensions: [
          {
            dimension: "Activities.startdate",
            dateRange: "This week"
          }
        ],
        order: {
          "Activities.startdate": "asc"
        },
        dimensions: [
          "Activities.startdate",
          "Activities.name",
          "Activities.type",
          "Activities.ElapsedTimeHumanize",
          "Activities.distance"
        ],
        filters: []
      },
      chartType: "table"
    }
  },
  {
    id: 5,
    name: "Shoe Usage Summary",
    vizState: {
      query: {
        measures: ["Activities.DistanceSum"],
        timeDimensions: [
          {
            dimension: "Activities.startdate"
          }
        ],
        order: {
          "Activities.DistanceSum": "desc"
        },
        dimensions: [
          "AthletesEquipmentShoes.equipmentShoesBrand",
          "AthletesEquipmentShoes.equipmentShoesModel",
          "AthletesEquipmentShoes.equipmentShoesDistance"
        ],
        filters: [
          {
            dimension: "AthletesEquipmentShoes.equipmentShoesDistance",
            operator: "set"
          }
        ]
      },
      chartType: "pie"
    }
  }
];

const DashboardPage = () => {
  const dashboardItem = item => (
    <Grid item xs={12} lg={6} key={item.id}>
      <DashboardItem title={item.name}>
        <ChartRenderer vizState={item.vizState} />
      </DashboardItem>
    </Grid>
  );

  const Empty = () => (
    <div
      style={{
        textAlign: "center",
        padding: 12
      }}
    >
      <Typography variant="h5" color="inherit">
        There are no charts on this dashboard. Use Playground Build to add one.
      </Typography>
    </div>
  );

  return DashboardItems.length ? (
    <Dashboard>{DashboardItems.map(dashboardItem)}</Dashboard>
  ) : (
    <Empty />
  );
};

export default DashboardPage;