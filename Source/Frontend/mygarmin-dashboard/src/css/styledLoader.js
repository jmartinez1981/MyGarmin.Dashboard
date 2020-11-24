import LoadingOverlay from 'react-loading-overlay';
import styled from 'styled-components'

const StyledLoader = styled(LoadingOverlay)`
            width: 100%;
            height: 100%;
            .MyLoader_overlay {
                color: black;
                background-color: #fafafa;
            }
            .MyLoader_spinner svg circle{
                stroke: rgba(166, 214, 204, 100);
            }
            .MyLoader_wrapper--active {
                overflow: hidden;
            }
            `;

export default StyledLoader;