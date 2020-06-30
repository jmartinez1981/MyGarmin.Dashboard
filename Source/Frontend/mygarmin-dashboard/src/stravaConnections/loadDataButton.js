import * as React from "react";
import { useUpdate, useNotify, useRedirect, Button } from 'react-admin';

const LoadDataButton = ({ record }) => {
    const notify = useNotify();
    const redirect = useRedirect();
    const [loadData, { loading }] = useUpdate(
        'stravaConnections',
        record.id,
        { },
        {
            undoable: true,
            onSuccess: ({ data }) => {
                redirect('/stravaConnections');
                notify('Data updated', 'info', {}, true);
            },
            onFailure: (error) => notify(`Error: ${error.message}`, 'warning'),
        }
    );
    return <Button label="Update data" onClick={loadData} disabled={loading} />;
};

export default LoadDataButton;