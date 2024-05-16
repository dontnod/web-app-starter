import { ClaimsData, createClaimsTable, IdTokenClaims } from '@/utils/claim-utils'
import { Table, TableColumnsType } from 'antd'
import { ReactNode } from 'react'

export interface ProfileDataProps {
  graphData: Record<string, ReactNode>
}

export const ProfileData = (props: ProfileDataProps) => {
  const tableRows = Object.entries(props.graphData).map((entry, index) => {
    return (
      <tr key={index}>
        <td>
          <b>{entry[0]}: </b>
        </td>
        <td>{entry[1]}</td>
      </tr>
    )
  })

  return (
    <>
      <div className="data-area-div">
        <p>
          Calling <strong>Microsoft Graph API</strong>...
        </p>
        <ul>
          <li>
            <strong>resource:</strong> <mark>User</mark> object
          </li>
          <li>
            <strong>endpoint:</strong> <mark>https://graph.microsoft.com/v1.0/me</mark>
          </li>
          <li>
            <strong>scope:</strong> <mark>user.read</mark>
          </li>
        </ul>
        <p>
          Contents of the <strong>response</strong> is below:
        </p>
      </div>
      <div className="data-area-div">
        <table>
          <thead></thead>
          <tbody>{tableRows}</tbody>
        </table>
      </div>
    </>
  )
}

export interface IdTokenDataProps {
  idTokenClaims: IdTokenClaims
}

export function IdTokenData(props: IdTokenDataProps) {
  const tokenClaims = createClaimsTable(props.idTokenClaims)

  const dataSource = Object.keys(tokenClaims).map((key) => {
    return tokenClaims[key]
  })

  const columns: TableColumnsType<ClaimsData> = [
    {
      title: 'Claim',
      dataIndex: 'claim',
      key: 'claim',
      ellipsis: true,
    },
    {
      title: 'Value',
      dataIndex: 'value',
      key: 'value',
      ellipsis: true,
    },
    {
      title: 'Description',
      dataIndex: 'description',
      key: 'description',
      ellipsis: true,
    },
  ]

  return (
    <>
      <div className="data-area-div">
        <p>
          See below the claims in your <strong> ID token </strong>. For more information, visit:{' '}
          <span>
            <a href="https://docs.microsoft.com/en-us/azure/active-directory/develop/id-tokens#claims-in-an-id-token">
              docs.microsoft.com
            </a>
          </span>
        </p>
        <div className="data-area-div">
          <Table dataSource={dataSource} columns={columns} rowKey={(row) => row.claim} />
        </div>
      </div>
    </>
  )
}
