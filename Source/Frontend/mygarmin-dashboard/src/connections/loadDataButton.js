import * as React from "react";
import { useMutation, useNotify, useRedirect, Button } from 'react-admin';

const LoadDataButton = ({ record }) => {
    const notify = useNotify();
    const redirect = useRedirect();
    const [loadData, { loading }] = useMutation(
        {
            type: 'update',
            resource: 'stravaConnections/loadData',
            payload: { id: record.id, data: { isApproved: true } },
        },
        {
            onSuccess: ({ data }) => {
                redirect('/stravaConnections');
                notify('Load data successfully');
            },
            onFailure: (error) => notify(`Load data was wrong with error: ${error.message}`, 'warning'),
        }
    );
    return <Button label="Load data" onClick={loadData} disabled={loading} />;
};

export default LoadDataButton;